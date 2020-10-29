using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Payment.API.StartupConfiguration
{
    public static class Swagger
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Winiety - Payment HTTP API",
                    Version = "v1",
                    Description = "Payment module for Winiety application.",
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerEx(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Payment Swagger UI";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment HTTP API V1");
            });

            return app;
        }
    }
}
