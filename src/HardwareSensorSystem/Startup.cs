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
        public Startup(IHostingEnvironment env, IConfiguration config)
        {
            HostingEnvironment = env;
            Configuration = config;
        }

        private IHostingEnvironment HostingEnvironment { get; }
        private IConfiguration Configuration { get; }

        /// <summary>
        /// This method add services to the container.
        /// </summary>
        /// <param name="services">Contract for a collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            void DbContextOptions(DbContextOptionsBuilder builder) => builder.UseSqlServer(
                Configuration.GetValue<string>("DATABASE"), b => b.MigrationsAssembly(GetType().Assembly.FullName));

            services.AddSecuriy(Configuration, DbContextOptions);

            services.AddSensorTechnology(DbContextOptions);

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
        public void Configure(IApplicationBuilder app)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseSecurity();

            app.UseMvcWithDefaultRoute();
        }
    }
}
