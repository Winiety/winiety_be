using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Payment.Core.Interfaces;
using Payment.Core.Mappings;
using Payment.Core.Services;
using Payment.Infrastructure.Repositories;
using Shared.Core.Services;

namespace Payment.API.StartupConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IWinietaRepository, WinietaRepository>();

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddHttpContextAccessor();

            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IUserContext, UserContext>();

            return services;
        }
    }
}
