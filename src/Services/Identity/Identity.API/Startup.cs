using HealthChecks.UI.Client;
using Identity.API.IdentityConfiguration;
using Identity.API.Options;
using Identity.API.StartupConfiguration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using System;

namespace Identity.API
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
            services.AddControllers();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddHttpClient();

            services.Configure<ReCaptchaOptions>(Configuration.GetSection("ReCaptchaOptions"));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(Configuration.GetValue<string>("ClientUrls:ReactApp"))
                    .AllowCredentials());
            });

            services
                .ConfigureHealthChecks(Configuration)
                .ConfigureIdentity(Configuration);
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

            retry.Execute(() => IdentitySeed.InitializeDatabase(app, Configuration));

            app.UseStaticFiles();

            app.UseForwardedHeaders();

            app.UseCors("CorsPolicy");

            app.UseIdentityServer();

            app.UseCookiePolicy(new CookiePolicyOptions { Secure = CookieSecurePolicy.Always, MinimumSameSitePolicy = SameSiteMode.None });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }
    }
}
