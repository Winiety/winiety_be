using Microsoft.Extensions.DependencyInjection;
using Rides.Core.Interfaces;
using Rides.Core.Services;
using Rides.Infrastructure.Repositories;

namespace Rides.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRideRepository, RideRepository>();

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IRideService, RideService>();

            return services;
        }
    }
}
