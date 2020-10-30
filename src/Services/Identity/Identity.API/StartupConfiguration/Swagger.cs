using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Identity.API.StartupConfiguration
{
    public static class Swagger
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Winiety - Identity HTTP API",
                    Version = "v1",
                    Description = "Identity module for Winiety application.",
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerEx(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Identity Swagger UI";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity HTTP API V1");
            });

            return app;
        }
    }
}
