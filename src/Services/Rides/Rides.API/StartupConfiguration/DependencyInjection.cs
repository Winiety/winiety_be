using Microsoft.Extensions.DependencyInjection;
using Rides.Application.Services;

namespace Rides.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IRidesService, RidesService>();

            return services;
        }
    }
}
