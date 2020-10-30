using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Rides.Application.Consumers;

namespace Rides.API.StartupConfiguration
{
    public static class MassTransit
    {
        public static IServiceCollection ConfigureMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(c =>
            {
                c.AddConsumer<CarRegisteredConsumer>();

                c.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint("rides-listener", e =>
                    {
                        e.ConfigureConsumer<CarRegisteredConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
