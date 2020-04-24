using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Statistic.Application.Statistic.GetUserStatistic;
using Statistic.Persistence;
using System;
using System.Collections.Generic;

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
            //services.AddSingleton<StatisticDbContext>();
            //services.AddScoped<IRepository<UserStatistic>, StatisticRepository>();
            //services.AddAutoMapper(typeof(QuizProfile).Assembly);
            services.AddMediatR(typeof(GetUserStatisticQueryHandler).Assembly);

            var identityUrl = Configuration["IdentityUrl"];

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "You api title",
                    Version = "v1"
                });
                //c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.AddSecurityDefinition("oauth2",
                    new OpenApiSecurityScheme()
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows()
                        {
                            Implicit = new OpenApiOAuthFlow()
                            {
                                AuthorizationUrl = new Uri($"{identityUrl}/connect/authorize"),
                                TokenUrl = new Uri($"{identityUrl}/connect/token"),
                                Scopes = new Dictionary<string, string>
                                {
                                    {"StatisticApi", "Statistic API - full access"}
                                },
                            }
                        }
                    });
            });

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        // base-address of your identityserver
            //        options.Authority = identityUrl;
            //        options.RequireHttpsMetadata = false;
            //        // name of the API resource
            //        options.Audience = "QuizApi";
            //        options.TokenValidationParameters.ValidIssuers = new[]
            //        {
            //            identityUrl
            //        };
            //    }).AddCookie();
            services.AddAuthorization(options => { });

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

            app.UseRouting();

            app.UseCors(builder => builder
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Content-Disposition"));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quiz API V1");
                c.OAuthClientId("SwaggerId");
                c.OAuthAppName("Swagger UI");
            });

            app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
