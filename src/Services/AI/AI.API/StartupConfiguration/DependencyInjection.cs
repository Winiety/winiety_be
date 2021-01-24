using AI.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AI.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAIService, AIService>();
            services.AddScoped(x => configuration.GetValue<string>("PredictService"));

            return services;
        }
    }
}
