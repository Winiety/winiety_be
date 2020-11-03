﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Rides.API.Filters;
using System;
using System.Collections.Generic;

namespace Rides.API.StartupConfiguration
{
    public static class Swagger
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Winiety - Rides HTTP API",
                    Version = "v1",
                    Description = "Rides module for Winiety application.",
                });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{configuration.GetValue<string>("IdentityUrl")}/connect/authorize"),
                            TokenUrl = new Uri($"{configuration.GetValue<string>("IdentityUrl")}/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                { "rides", "Rides API" }
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
                c.DocumentTitle = "Rides Swagger UI";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rides HTTP API V1");
                c.OAuthClientId("ridesswaggerui");
                c.OAuthAppName("Rides Swagger UI");
            });

            return app;
        }
    }
}
