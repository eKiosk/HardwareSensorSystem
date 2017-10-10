using HardwareSensorSystem.Security.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace HardwareSensorSystem.Security
{
    public static class Database
    {
        public static void Initialize(IServiceProvider services)
        {
            var configurationDbContext = services.GetRequiredService<ConfigurationDbContext>();
            if (!configurationDbContext.Clients.Any())
            {
                foreach (var client in Config.GetClients())
                {
                    configurationDbContext.Clients.Add(client.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }

            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            if (!roleManager.Roles.Any())
            {
            }

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
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
    }
}
