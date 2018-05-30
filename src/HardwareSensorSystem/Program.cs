using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.SensorTechnology.Models;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace HardwareSensorSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetService<IConfiguration>();

                try
                {
                    services.GetService<ApplicationDbContext>().Database.Migrate();
                    services.GetService<ConfigurationDbContext>().Database.Migrate();
                    services.GetService<PersistedGrantDbContext>().Database.Migrate();
                    services.GetService<SensorTechnologyDbContext>().Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred migrating the database.");
                }

                if (configuration.GetValue<bool>("DATABASE_SEED"))
                {
                    try
                    {
                        Security.Database.Initialize(services);
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred seeding the database.");
                    }
                }
            }

            host.Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration(config =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", true)
                          .AddEnvironmentVariables();
                })
                .Build();
    }
}
