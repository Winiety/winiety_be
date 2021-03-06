﻿using Contracts.Commands;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Statistics.API.StartupConfiguration
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
                });

                c.AddRequestClient<GetRideDates>();
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
