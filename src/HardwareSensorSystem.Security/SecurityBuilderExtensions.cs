using HardwareSensorSystem.Security;
using HardwareSensorSystem.Security.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Microsoft.AspNetCore.Builder
{
    public static class SecurityBuilderExtensions
    {
        public static IApplicationBuilder UseSecurity(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseIdentityServer();

            return app;
        }

        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                if (!configurationDbContext.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        configurationDbContext.Clients.Add(client.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                if (!roleManager.Roles.Any())
                {
                }

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                if (!userManager.Users.Any())
                {
                    foreach (var user in Config.GetUsers())
                    {
                        ApplicationUser dbUser = new ApplicationUser
                        {
                            UserName = user.UserName
                        };
                        userManager.CreateAsync(dbUser, user.Password).Wait();
                    }
                }
            }

            return app;
        }
    }
}
