using AutoMapper;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pictures.Core.Interfaces;
using Pictures.Core.Mappings;
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

        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MapperProfile));

            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<IPictureService, PictureService>();

            services.AddScoped(x => new BlobServiceClient(configuration.GetValue<string>("AzureBlobKey")));

            return services;
        }
    }
}
