using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Profile.Core.Interfaces;
using Profile.Core.Mappings;
using Profile.Core.Services;
using Profile.Infrastructure.Repositories;
using Shared.Core.Services;

namespace Profile.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICarRepository, CarRepository>();
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddHttpContextAccessor();

            services.AddTransient<ICarService, CarService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddTransient<IUserContext, UserContext>();

            return services;
        }
    }
}
