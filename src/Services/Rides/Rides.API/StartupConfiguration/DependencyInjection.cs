using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Rides.Core.Interfaces;
using Rides.Core.Mappings;
using Rides.Core.Services;
using Rides.Infrastructure.Repositories;
using Shared.Core.Services;

namespace Rides.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRideRepository, RideRepository>();

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddHttpContextAccessor();

            services.AddTransient<IRideService, RideService>();
            services.AddTransient<IUserContext, UserContext>();

            return services;
        }
    }
}
