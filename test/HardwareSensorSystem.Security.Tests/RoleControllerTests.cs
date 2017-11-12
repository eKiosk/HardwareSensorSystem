using HardwareSensorSystem.Security.Controllers;
using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HardwareSensorSystem.Security.Tests
{
    public class RoleControllerTests
    {
        [Fact]
        public async Task GetAll_WhenCalled_ReturnsCollectionOfRolesFromRoleManager()
        {
            // Arrange
            var testRoles = GetRoles();
            var mockRoleManager = new Mock<RoleManager<ApplicationRole>>(
                new Mock<IRoleStore<ApplicationRole>>().Object,
                new List<IRoleValidator<ApplicationRole>>(),
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<ILogger<RoleManager<ApplicationRole>>>().Object);
            mockRoleManager.Setup(roleManager => roleManager.Roles).Returns(testRoles.AsQueryable());
            var controller = new RoleController(mockRoleManager.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var roles = Assert.IsAssignableFrom<IEnumerable<RoleViewModel>>(okObjectResult.Value);
            Assert.All(testRoles, testRole =>
            {
                var role = roles.SingleOrDefault(p => p.Id == testRole.Id);
                Assert.NotNull(role);
                Assert.Equal(testRole.Name, role.Name);
            });
        }

        private static IEnumerable<ApplicationRole> GetRoles()
        {
            return new List<ApplicationRole>()
            {
                new ApplicationRole()
                {
                    Id = 1,
                    Name = "Admin"
                },
                new ApplicationRole()
                {
                    Id = 2,
                    Name = "User"
                },
                new ApplicationRole()
                {
                    Id = 3,
                    Name = "Demo"
                }
            };
        }
    }
}
