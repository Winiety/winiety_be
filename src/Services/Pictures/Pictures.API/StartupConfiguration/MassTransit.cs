using Contracts.Commands;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Pictures.API.StartupConfiguration
{
    public static class MassTransit
    {
        public static IServiceCollection ConfigureMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();

                x.AddRequestClient<AnalyzePicture>();
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
