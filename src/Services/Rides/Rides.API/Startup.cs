using Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Rides.API.StartupConfiguration;
using Rides.Infrastructure.Data;
using System;

namespace Rides.API
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

            services
                .ConfigureSwagger(Configuration)
                .ConfigureServices()
                .ConfigureRepositories()
                .ConfigureHealthChecks(Configuration)
                .ConfigureMassTransit(Configuration)
                .AddDb(Configuration)
                .ConfigureAuthentication(Configuration);
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
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                if (context.Rides.CountAsync().Result < 100)
                {
                    var picId = 300;
                    var faker = new Faker<Core.Model.Entities.Ride>()
                        .RuleFor(c => c.PlateNumber, f => f.Lorem.Letter(8))
                        .RuleFor(c => c.PictureId, f => picId++)
                        .RuleFor(c => c.Speed, f => f.Random.Double(100, 150))
                        .RuleFor(c => c.UserId, f => null)
                        .RuleFor(c => c.RideDateTime, f => f.Date.Between(DateTime.Now.AddYears(-3), DateTime.Now));

                    context.Rides.AddRange(faker.Generate(100000));
                }
                context.SaveChanges();
            }
        }
    }
}
