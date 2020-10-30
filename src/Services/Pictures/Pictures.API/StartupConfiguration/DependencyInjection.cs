using Microsoft.Extensions.DependencyInjection;
using Pictures.Application.Services;

namespace Pictures.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IPictureService, PictureService>();

            return services;
        }
    }
}
