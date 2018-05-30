using HardwareSensorSystem.Security.Models;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;

namespace Microsoft.AspNetCore.Builder
{
    public static class SecurityServiceCollectionExtensions
    {
        public static void AddSecuriy(this IServiceCollection services, IConfiguration configuration, Action<DbContextOptionsBuilder> dbContextOptionsAction)
        {
            services.AddDbContext<ApplicationDbContext>(dbContextOptionsAction);

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 3;

                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 8;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var rsaKey = new RsaSecurityKey(RSA.Create());
            services.AddIdentityServer()
                .AddSigningCredential(rsaKey)
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = dbContextOptionsAction;
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = dbContextOptionsAction;
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 300;
                });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                })
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration.GetValue<string>("AUTHORITY");
                    options.RequireHttpsMetadata = false;
                    options.SupportedTokens = SupportedTokens.Jwt;
                });
        }
    }
}
