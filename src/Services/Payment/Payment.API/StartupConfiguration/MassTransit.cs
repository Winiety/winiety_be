using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Core.Consumers;

namespace Payment.API.StartupConfiguration
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

                    cfg.ReceiveEndpoint("payments-listener", e =>
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
