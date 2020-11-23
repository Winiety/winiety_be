using Microsoft.Extensions.DependencyInjection;
using Statistics.Core.Services;

namespace Statistics.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IStatisticsService, StatisticsService>();

            return services;
        }
    }
}
