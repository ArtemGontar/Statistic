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
            
            services.AddAutoMapper(typeof(StatisticProfile).Assembly);
            services.AddMediatR(typeof(GetUserStatisticQueryHandler).Assembly);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<DeleteChapterConsumer>();

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
                }));
            });
            services.AddMassTransitHostedService();

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddMongoDb(Configuration.GetSection(ConnectionStrings.SECTION_NAME).Get<ConnectionStrings>().Mongo);


            var identityUrl = Configuration["IdentityUrl"];

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "You api title",
                    Version = "v1"
                });
            });

            services.AddCors(options =>
                options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();


            app.UseCors("AllowAll");
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quiz API V1");
                c.OAuthClientId("SwaggerId");
                c.OAuthAppName("Swagger UI");
            });

            app.UseAuthentication();

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
    }
}
