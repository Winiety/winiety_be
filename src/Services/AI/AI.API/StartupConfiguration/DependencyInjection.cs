using AI.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AI.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IAIService, AIService>();

            return services;
        }
    }
}
