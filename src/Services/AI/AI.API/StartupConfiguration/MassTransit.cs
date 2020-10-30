using AI.Application.Consumers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace AI.API.StartupConfiguration
{
    public static class MassTransit
    {
        public static IServiceCollection ConfigureMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(c =>
            {
                c.AddConsumer<AnalyzePictureConsumer>();

                c.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint("ai-listener", e =>
                    {
                        e.ConfigureConsumer<AnalyzePictureConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
