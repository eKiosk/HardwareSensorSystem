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
using System.Collections.Generic;

namespace HardwareSensorSystem
{
    public class Program
    {
        protected Program() { }

        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetService<IConfiguration>();

                if (configuration.GetValue<bool>("DATABASE_MIGRATE"))
                {
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

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration(config =>
                {
                    var defaults = new Dictionary<string, string>
                    {
                        {"AUTHORITY", "http://localhost:5000/"},
                        {"DATABASE", "Server=localhost;Database=tempdb;User Id=sa;Password=msSql_password;"},
                        {"DATABASE_MIGRATE", "False"},
                        {"DATABASE_SEED", "False"}
                    };
                    config.AddInMemoryCollection(defaults);

                    config.AddEnvironmentVariables();
                })
                .Build();
    }
}
