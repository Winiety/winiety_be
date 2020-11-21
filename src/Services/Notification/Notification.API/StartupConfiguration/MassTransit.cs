using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Core.Consumers;

namespace Notification.API.StartupConfiguration
{
    public static class MassTransit
    {
        public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(c =>
            {
                c.AddConsumer<RideRegisteredConsumer>();

                c.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMqHost"]);
                    cfg.ReceiveEndpoint("notifications-listener", e =>
                    {
                        e.ConfigureConsumer<RideRegisteredConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
