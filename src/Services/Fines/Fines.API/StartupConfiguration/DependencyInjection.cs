using Microsoft.Extensions.DependencyInjection;

namespace Fines.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
