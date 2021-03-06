﻿using Fines.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace Fines.API.StartupConfiguration
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
                    Title = "Winiety - Fines HTTP API",
                    Version = "v1",
                    Description = "Fines and complaints module for Winiety application.",
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
                                { "fines", "Fines API" }
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
                c.DocumentTitle = "Fines Swagger UI";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fines HTTP API V1");
                c.OAuthClientId("finesswaggerui");
                c.OAuthAppName("Fines Swagger UI");
            });

            return app;
        }
    }
}
