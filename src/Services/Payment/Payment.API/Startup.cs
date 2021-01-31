using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Payment.API.StartupConfiguration;
using Payment.Core.Options;
using Payment.Infrastructure.Data;
using Polly;
using System;

namespace Payment.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(options => options.EnableEndpointRouting = false)
                .AddControllersAsServices();

            services.Configure<PayuOptions>(Configuration.GetSection("PAYU"));

            services
                .ConfigureSwagger(Configuration)
                .ConfigureRepositories()
                .ConfigureServices()
                .ConfigureHealthChecks(Configuration)
                .ConfigureMassTransit(Configuration)
                .ConfigureAuthentication(Configuration)
                .AddDb(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var retry = Policy.Handle<SqlException>()
               .WaitAndRetry(new TimeSpan[]
               {
                            TimeSpan.FromSeconds(2),
                            TimeSpan.FromSeconds(4),
                            TimeSpan.FromSeconds(8),
                            TimeSpan.FromSeconds(16),
                            TimeSpan.FromSeconds(32),
                            TimeSpan.FromSeconds(64),
                            TimeSpan.FromSeconds(128),
                            TimeSpan.FromSeconds(256),
                            TimeSpan.FromSeconds(512),
                            TimeSpan.FromSeconds(1024),
                            TimeSpan.FromSeconds(2048),
               });

            retry.Execute(() => InitializeDatabase(app));

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMvcWithDefaultRoute();

            app.UseHealthChecks();

            app.UseSwaggerEx();
        }

        public void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            }
        }
    }
}
