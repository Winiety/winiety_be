using AutoMapper;
using Fines.Core.Interfaces;
using Fines.Core.Mappings;
using Fines.Core.Services;
using Fines.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Services;

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
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddHttpContextAccessor();

            services.AddTransient<IFineService, FineService>();
            services.AddTransient<IComplaintService, ComplaintService>();
            services.AddTransient<IUserContext, UserContext>();

            return services;
        }
    }
}
