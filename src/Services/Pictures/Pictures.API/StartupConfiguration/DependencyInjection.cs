using Microsoft.Extensions.DependencyInjection;
using Pictures.Core.Interfaces;
using Pictures.Core.Services;
using Pictures.Infrastructure.Repositories;

namespace Pictures.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPictureRepository, PictureRepository>();

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IPictureService, PictureService>();

            return services;
        }
    }
}
