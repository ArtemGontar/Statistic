using AutoMapper;
using GreenPipes;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shared.Persistence.MongoDb;
using Statistic.Application.Infrastructure;
using Statistic.Application.Statistic.GetUserStatistic;
using Statistic.Domain;
using Statistic.Persistence;
using System;
using HealthChecks.UI.Client;
using Statistic.Application.Consumers;
using Statistic.Application.UserStatistic.GetUserStatistic;
using Statistic.Application.Services;
using OpenTracing;
using Jaeger;
using Jaeger.Samplers;
using OpenTracing.Util;

namespace Statistic
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ConnectionStrings>(Configuration.GetSection(ConnectionStrings.SECTION_NAME));
            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(2);
                options.Predicate = (check) => check.Tags.Contains("ready");
            });

            services.AddSingleton<StatisticDbContext>();
            services.AddScoped<IRepository<UserStatistic>, UserStatisticRepository>();
            services.AddScoped<IRepository<QuizStatistic>, QuizStatisticRepository>();

            services.AddScoped<IUserStatisticService, UserStatisticService>();
            services.AddScoped<IQuizStatisticService, QuizStatisticService>();

            services.AddAutoMapper(typeof(StatisticProfile).Assembly);
            services.AddMediatR(typeof(GetUserStatisticQueryHandler).Assembly);

            AppMassTransit(services);

            services.AddOpenTracing();

            services.AddSingleton<ITracer>(serviceProvider =>
            {
                string serviceName = serviceProvider.GetRequiredService<IWebHostEnvironment>().ApplicationName;

                // This will log to a default localhost installation of Jaeger.
                var tracer = new Tracer.Builder(serviceName)
                    .WithSampler(new ConstSampler(true))
                    .Build();

                // Allows code that can't use DI to also access the tracer.
                GlobalTracer.Register(tracer);

                return tracer;
            });

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddMongoDb(Configuration.GetSection(ConnectionStrings.SECTION_NAME).Get<ConnectionStrings>().Mongo);

            services.AddCors(options =>
                options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));

            var identityUrl = Configuration["IdentityUrl"];

            AppConfigureAuth(services, identityUrl);

            AppConfigureSwagger(services);

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowAll");
            UseConfigureAuth(app);
            UseConfigureSwagger(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });

                endpoints.MapHealthChecks("/readiness", new HealthCheckOptions
                {
                    Predicate = r => !r.Name.Contains("self")
                });

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                {
                    // Exclude all checks and return a 200-Ok.
                    Predicate = (_) => false
                });
            });
        }

        protected virtual void AppConfigureAuth(IServiceCollection services, string identityUrl)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = identityUrl;
                    options.RequireHttpsMetadata = false;
                    options.Audience = "StaticticApi";
                    options.TokenValidationParameters.ValidIssuers = new[]
                    {
                        identityUrl
                    };
                }).AddCookie();


            services.AddAuthorization();
        }

        protected virtual void AppMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<DeleteChapterConsumer>();
                x.AddConsumer<QuizResultConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    // configure health checks for this bus instance
                    cfg.UseHealthCheck(provider);

                    cfg.Host("rabbitmq://localhost");

                    cfg.ReceiveEndpoint("delete-chapter", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));

                        ep.ConfigureConsumer<DeleteChapterConsumer>(provider);
                    });

                    cfg.ReceiveEndpoint("quiz-result", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));

                        ep.ConfigureConsumer<QuizResultConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();
        }

        protected virtual void AppConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "You api title",
                    Version = "v1"
                });
                //c.OperationFilter<AuthorizeCheckOperationFilter>();
                //c.AddSecurityDefinition("oauth2",
                //    new OpenApiSecurityScheme()
                //    {
                //        Type = SecuritySchemeType.OAuth2,
                //        Flows = new OpenApiOAuthFlows()
                //        {
                //            Implicit = new OpenApiOAuthFlow()
                //            {
                //                AuthorizationUrl = new Uri($"{identityUrl}/connect/authorize"),
                //                TokenUrl = new Uri($"{identityUrl}/connect/token"),
                //                Scopes = new Dictionary<string, string>
                //                {
                //                    {"QuizApi", "Quiz API - full access"}
                //                },
                //            }
                //        }
                //    });
            });
        }

        protected virtual void UseConfigureAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        protected virtual void UseConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Statistic API V1");
                c.OAuthClientId("SwaggerId");
                c.OAuthAppName("Swagger UI");
            });
        }
    }
}
