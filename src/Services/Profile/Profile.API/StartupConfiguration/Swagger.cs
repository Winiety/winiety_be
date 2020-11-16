using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Profile.API.Filters;
using System;
using System.Collections.Generic;

namespace Profile.API.StartupConfiguration
{
    public static class Swagger
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("IdentityClientUrl") ?? configuration.GetValue<string>("IdentityUrl");

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Winiety - Profile HTTP API",
                    Version = "v1",
                    Description = "Profile module for Winiety application.",
                });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{url}/connect/authorize"),
                            TokenUrl = new Uri($"{url}/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                { "userprofile", "Profile API" }
                            }
                        }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerEx(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Profile Swagger UI";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Profile HTTP API V1");
                c.OAuthClientId("userprofileswaggerui");
                c.OAuthAppName("Profile Swagger UI");
            });

            return app;
        }
    }
}
