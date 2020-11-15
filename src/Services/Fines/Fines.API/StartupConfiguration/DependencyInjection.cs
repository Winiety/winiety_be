using Fines.Core.Interfaces;
using Fines.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Fines.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<IFineRepository, FineRepository>();
            services.AddTransient<IComplaintRepository, ComplaintRepository>();

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
