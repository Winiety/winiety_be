using Microsoft.Extensions.DependencyInjection;

namespace Statistics.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
