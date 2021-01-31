using Identity.Core.Data;
using Identity.Core.Data.Model;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Identity.API.IdentityConfiguration
{
    public static class IdentitySeed
    {
        public static void InitializeDatabase(IApplicationBuilder app, IConfiguration configuration)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var appContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                appContext.Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                var clientUrls = new Dictionary<string, string>();

                clientUrls.Add("ReactApp", configuration.GetValue<string>("ClientUrls:ReactApp"));
                clientUrls.Add("AIApi", configuration.GetValue<string>("ClientUrls:AIApi"));
                clientUrls.Add("FinesApi", configuration.GetValue<string>("ClientUrls:FinesApi"));
                clientUrls.Add("PaymentApi", configuration.GetValue<string>("ClientUrls:PaymentApi"));
                clientUrls.Add("PicutresApi", configuration.GetValue<string>("ClientUrls:PicutresApi"));
                clientUrls.Add("RidesApi", configuration.GetValue<string>("ClientUrls:RidesApi"));
                clientUrls.Add("StatisticsApi", configuration.GetValue<string>("ClientUrls:StatisticsApi"));
                clientUrls.Add("ProfileApi", configuration.GetValue<string>("ClientUrls:ProfileApi"));
                clientUrls.Add("NotificationApi", configuration.GetValue<string>("ClientUrls:NotificationApi"));


                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityConfiguration.GetClients(clientUrls))
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityConfiguration.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var scope in IdentityConfiguration.GetApiScopes())
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in IdentityConfiguration.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!roleManager.Roles.Any())
                {
                    foreach (var role in IdentityConfiguration.GetRoles())
                    {
                        _ = roleManager.CreateAsync(role).Result;
                    }
                }

                if (!appContext.Users.Any())
                {
                    AddUser(userManager, "jan.user@winiety.com", "Jan", "User", "P@ssw0rd", "user");
                    AddUser(userManager, "jan.admin@winiety.com", "Jan", "Admin", "P@ssw0rd", "admin");
                    AddUser(userManager, "jan.corrector@winiety.com", "Jan", "Corrector", "P@ssw0rd", "corrector");
                    AddUser(userManager, "jan.analyst@winiety.com", "Jan", "Analyst", "P@ssw0rd", "analyst");
                    AddUser(userManager, "jan.police@winiety.com", "Jan", "Police", "P@ssw0rd", "police");
                }
            }
        }

        private static void AddUser(UserManager<ApplicationUser> userManager, string email, string firstName, string lastName, string password, string role)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
            };

            _ = userManager.CreateAsync(user, password).Result;
            _ = userManager.AddToRoleAsync(user, role).Result;
        }
    }
}
