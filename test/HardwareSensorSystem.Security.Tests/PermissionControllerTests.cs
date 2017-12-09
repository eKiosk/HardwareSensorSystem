using HardwareSensorSystem.Security.Controllers;
using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HardwareSensorSystem.Security.Tests
{
    public class PermissionControllerTests
    {
        [Fact]
        public async Task GetAll_WhenCalled_ReturnsCollectionOfPermissionsFromDatabase()
        {
            // Arrange
            var testPermissions = GetPermissions();
            var dbContext = Setup.GetDbContext();
            dbContext.Permissions.AddRange(testPermissions);
            dbContext.SaveChanges();
            var controller = new PermissionController(dbContext);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var permissions = Assert.IsAssignableFrom<IEnumerable<PermissionViewModel>>(okObjectResult.Value);
            Assert.All(testPermissions, testPermission =>
            {
                var permission = permissions.SingleOrDefault(p => p.Id == testPermission.Id);
                Assert.NotNull(permission);
                Assert.Equal(testPermission.Name, permission.Name);
            });
        }

        private static IEnumerable<ApplicationPermission> GetPermissions()
        {
            return new List<ApplicationPermission>()
            {
                new ApplicationPermission()
                {
                    Id = 1,
                    Name = "Read"
                },
                new ApplicationPermission()
                {
                    Id = 2,
                    Name = "Write"
                },
                new ApplicationPermission()
                {
                    Id = 3,
                    Name = "Delete"
                }
            };
        }
    }
}
