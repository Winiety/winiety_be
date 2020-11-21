using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Notification.Core.Interfaces;
using Notification.Core.Mappings;
using Notification.Core.Services;
using Notification.Infrastructure.Repositories;
using Shared.Core.Services;

namespace Notification.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<INotificationRepository, NotificationRepository>();

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddHttpContextAccessor();

            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IUserContext, UserContext>();

            return services;
        }
    }
}
