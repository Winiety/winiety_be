using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Payment.API.StartupConfiguration
{
    public static class MassTransit
    {
        public static IServiceCollection ConfigureMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
