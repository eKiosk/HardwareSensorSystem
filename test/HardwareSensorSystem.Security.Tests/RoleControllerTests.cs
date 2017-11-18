using HardwareSensorSystem.Security.Controllers;
using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
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
            var mockRoleManager = GetRoleManagerMock();
            var dbContext = GetDbContext();
            dbContext.Roles.AddRange(testRoles);
            dbContext.SaveChanges();
            mockRoleManager.Setup(roleManager => roleManager.Roles).Returns(dbContext.Roles);
            var controller = new RoleController(mockRoleManager.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var roles = Assert.IsAssignableFrom<IEnumerable<RoleViewModel>>(okObjectResult.Value);
            Assert.All(testRoles, testRole =>
            {
                var role = roles.SingleOrDefault(r => r.Id == testRole.Id);
                Assert.NotNull(role);
                Assert.Equal(testRole.Name, role.Name);
            });
        }

        [Fact]
        public async Task Create_WithValidRole_ReturnsCreatedRole()
        {
            // Arrange
            var mockRoleManager = GetRoleManagerMock();
            var controller = new RoleController(mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.CreateAsync(It.IsAny<ApplicationRole>())).ReturnsAsync((ApplicationRole appRole) =>
            {
                appRole.Id = 1;
                return new IdentityResult();
            }).Verifiable();
            var roleName = "RoleName";

            // Act
            var result = await controller.Create(new RoleViewModel() { Name = roleName });

            // Assert
            mockRoleManager.Verify();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var role = Assert.IsAssignableFrom<RoleViewModel>(okObjectResult.Value);
            Assert.Equal(1, role.Id);
            Assert.Equal(roleName, role.Name);
        }

        [Fact]
        public async Task Create_WithInvalidRole_ReturnsModelError()
        {
            // Arrange
            var mockRoleManager = GetRoleManagerMock();
            var controller = new RoleController(mockRoleManager.Object);
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Create(new RoleViewModel());

            // Assert
            mockRoleManager.Verify(roleManager => roleManager.CreateAsync(It.IsAny<ApplicationRole>()), Times.Never());
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
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

        private static ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        private static Mock<RoleManager<ApplicationRole>> GetRoleManagerMock()
        {
            return new Mock<RoleManager<ApplicationRole>>(
                new Mock<IRoleStore<ApplicationRole>>().Object,
                new List<IRoleValidator<ApplicationRole>>(),
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<ILogger<RoleManager<ApplicationRole>>>().Object);
        }
    }
}
