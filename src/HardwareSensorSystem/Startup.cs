using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HardwareSensorSystem
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// This method add services to the container.
        /// </summary>
        /// <param name="services">Contract for a collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSecuriy(_configuration, options =>
            {
                options.UseSqlServer(_configuration.GetValue<string>("DATABASE"), b => b.MigrationsAssembly("HardwareSensorSystem"));
            });

            services.AddSensorTechnology(options =>
            {
                options.UseSqlServer(_configuration.GetValue<string>("DATABASE"), b => b.MigrationsAssembly("HardwareSensorSystem"));
            });

            services.AddMvc(options =>
                {
                    var authorizationPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    options.Filters.Add(new AuthorizeFilter(authorizationPolicy));
                }
            );
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

            app.UseMvcWithDefaultRoute();
        }
    }
}
