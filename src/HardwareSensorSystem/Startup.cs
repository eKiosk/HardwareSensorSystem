using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HardwareSensorSystem
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup()
        {
            _configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// This method add services to the container.
        /// </summary>
        /// <param name="services">Contract for a collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration.GetValue<string>("DATABASE");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Server=localhost;Database=tempdb;User Id=sa;Password=msSql_password;";
            }
            services.AddSecuriy(options =>
            {
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("HardwareSensorSystem"));
            });
        }

        /// <summary>
        /// This method configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Class that provides the mechanisms to configure an application's request pipeline.</param>
        /// <param name="env">Information about the web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSecurity();
        }
    }
}
