using Contracts.Commands;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fines.API.StartupConfiguration
{
    public static class MassTransit
    {
        public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(c =>
            {
                c.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMqHost"]);
                    cfg.ReceiveEndpoint("fines-listener", e =>
                    {
                    });
                });
                
                c.AddRequestClient<GetRide>();
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
