using HardwareSensorSystem.Security.Models;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace HardwareSensorSystem.Security
{
    public static class Config
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "hardwaresensorsystem",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = false,
                    AllowOfflineAccess = true
                }
            };
        }

        public static IEnumerable<ApplicationPermission> GetPermissions()
        {
            return new List<ApplicationPermission>
            {
                new ApplicationPermission
                {
                    Name = "devices"
                },
                new ApplicationPermission
                {
                    Name = "users"
                }
            };
        }

        public static IEnumerable<ConfigApplicationUser> GetUsers()
        {
            return new List<ConfigApplicationUser>
            {
                new ConfigApplicationUser
                {
                    UserName = "Admin",
                    Password = "12345678"
                }
            };
        }
    }

    public class ConfigApplicationUser : ApplicationUser
    {
        public string Password { get; set; }
    }
}
