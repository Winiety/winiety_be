using Contracts.Commands;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rides.Core.Consumers;

namespace Rides.API.StartupConfiguration
{
    public static class MassTransit
    {
        public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(c =>
            {
                c.AddConsumer<CarRegisteredConsumer>();
                c.AddConsumer<GetRideDatesConsumer>();
                c.AddConsumer<GetRideConsumer>();

                c.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMqHost"]);
                    cfg.ReceiveEndpoint("rides-listener", e =>
                    {
                        e.ConfigureConsumer<CarRegisteredConsumer>(context);
                        e.ConfigureConsumer<GetRideDatesConsumer>(context);
                        e.ConfigureConsumer<GetRideConsumer>(context);
                    });
                });

                c.AddRequestClient<GetUserIdByPlate>();
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
