using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;

namespace ApiGateway
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
            var identityUrl = Configuration.GetValue<string>("identityUrl");

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddUrlGroup(new Uri(Configuration["AIUrlHC"]), name: "aiapi-check", tags: new string[] { "aiapi" })
                .AddUrlGroup(new Uri(Configuration["FinesUrlHC"]), name: "finesapi-check", tags: new string[] { "finesapi" })
                .AddUrlGroup(new Uri(Configuration["IdentityUrlHC"]), name: "identityapi-check", tags: new string[] { "identityapi" })
                .AddUrlGroup(new Uri(Configuration["PaymentUrlHC"]), name: "paymentapi-check", tags: new string[] { "paymentapi" })
                .AddUrlGroup(new Uri(Configuration["PicturesUrlHC"]), name: "picturesapi-check", tags: new string[] { "picturesapi" })
                .AddUrlGroup(new Uri(Configuration["RidesUrlHC"]), name: "ridesapi-check", tags: new string[] { "ridesapi" })
                .AddUrlGroup(new Uri(Configuration["StatisticsUrlHC"]), name: "statisticsapi-check", tags: new string[] { "statisticsapi" });

            services.AddAuthentication()
               .AddJwtBearer("IdentityApiKey", options =>
               {
                   options.Authority = identityUrl;
                   options.RequireHttpsMetadata = false;
                   options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                   {
                       ValidAudiences = new[] { "ai", "fines", "payment", "pictures", "rides", "statistics" }
                   };

//                   options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
//                   {
//#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
//#pragma warning disable CS0219 // Variable is assigned but its value is never used
//                       OnAuthenticationFailed = async ctx =>
//                       {
//                           int i = 0;
//                       },
//                       OnTokenValidated = async ctx =>
//                       {
//                           int i = 0;
//                       },

//                       OnMessageReceived = async ctx =>
//                       {
//                           int i = 0;
//                       }
//#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
//#pragma warning restore CS0219 // Variable is assigned but its value is never used
//                   };
               });

            services.AddOcelot();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health/ready", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });


            app.UseOcelot().Wait();
        }
    }
}
