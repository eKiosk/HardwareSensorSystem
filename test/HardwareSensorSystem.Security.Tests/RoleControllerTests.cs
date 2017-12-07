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
using System.Security.Claims;
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
                Assert.Equal(testRole.ConcurrencyStamp, role.ConcurrencyStamp);
            });
        }

        [Fact]
        public async Task GetById_WithRoleId_ReturnsRole()
        {
            // Arrange
            var testRole = new ApplicationRole()
            {
                Id = 10,
                Name = "RoleName",
                ConcurrencyStamp = "RoleStamp"
            };
            var mockRoleManager = GetRoleManagerMock();
            mockRoleManager.Setup(roleManager => roleManager.FindByIdAsync(
                    It.Is<string>(roleId => roleId.Equals(testRole.Id.ToString()))
                )).ReturnsAsync(testRole);
            var controller = new RoleController(mockRoleManager.Object);

            // Act
            var result = await controller.GetById(testRole.Id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var role = Assert.IsAssignableFrom<RoleViewModel>(okObjectResult.Value);
            Assert.Equal(testRole.Id, role.Id);
            Assert.Equal(testRole.Name, role.Name);
            Assert.Equal(testRole.ConcurrencyStamp, role.ConcurrencyStamp);
        }

        [Fact]
        public async Task Create_WithValidRole_ReturnsCreatedRole()
        {
            // Arrange
            var roleName = "RoleName";
            var roleConcurrencyStamp = "RoleStamp";
            var mockRoleManager = GetRoleManagerMock();
            var controller = new RoleController(mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.CreateAsync(It.IsAny<ApplicationRole>())).ReturnsAsync((ApplicationRole appRole) =>
            {
                appRole.Id = 1;
                appRole.ConcurrencyStamp = roleConcurrencyStamp;
                return IdentityResult.Success;
            }).Verifiable();

            // Act
            var result = await controller.Create(new RoleViewModel() { Name = roleName });

            // Assert
            mockRoleManager.Verify();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var role = Assert.IsAssignableFrom<RoleViewModel>(okObjectResult.Value);
            Assert.Equal(1, role.Id);
            Assert.Equal(roleName, role.Name);
            Assert.Equal(roleConcurrencyStamp, role.ConcurrencyStamp);
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

        [Fact]
        public async Task Create_WithUnprocessableRole_ReturnsCollectionOfIdentityErrors()
        {
            // Arrange
            var mockRoleManager = GetRoleManagerMock();
            var controller = new RoleController(mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.CreateAsync(It.IsAny<ApplicationRole>())).ReturnsAsync(IdentityResult.Failed()).Verifiable();

            // Act
            var result = await controller.Create(new RoleViewModel());

            // Assert
            mockRoleManager.Verify();
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<IdentityError>>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Update_WithValidRole_ReturnsUpdatedRole()
        {
            // Arrange
            var roleId = 10;
            var roleName = "RoleName";
            var roleConcurrencyStampOld = "OldRoleStamp";
            var roleConcurrencyStampNew = "NewRoleStamp";
            var mockRoleManager = GetRoleManagerMock();
            var controller = new RoleController(mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.UpdateAsync(It.IsAny<ApplicationRole>())).ReturnsAsync((ApplicationRole appRole) =>
            {
                appRole.ConcurrencyStamp = roleConcurrencyStampNew;
                return IdentityResult.Success;
            }).Verifiable();

            // Act
            var result = await controller.Update(roleId, new RoleViewModel()
            {
                Name = roleName,
                ConcurrencyStamp = roleConcurrencyStampOld
            });

            // Assert
            mockRoleManager.Verify();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var role = Assert.IsAssignableFrom<RoleViewModel>(okObjectResult.Value);
            Assert.Equal(roleId, role.Id);
            Assert.Equal(roleName, role.Name);
            Assert.Equal(roleConcurrencyStampNew, role.ConcurrencyStamp);
        }

        [Fact]
        public async Task Update_WithInvalidRole_ReturnsModelError()
        {
            // Arrange
            var mockRoleManager = GetRoleManagerMock();
            var controller = new RoleController(mockRoleManager.Object);
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Update(1, new RoleViewModel());

            // Assert
            mockRoleManager.Verify(roleManager => roleManager.UpdateAsync(It.IsAny<ApplicationRole>()), Times.Never());
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Update_WithUnprocessableRole_ReturnsCollectionOfIdentityErrors()
        {
            // Arrange
            var mockRoleManager = GetRoleManagerMock();
            var controller = new RoleController(mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.UpdateAsync(It.IsAny<ApplicationRole>())).ReturnsAsync(IdentityResult.Failed()).Verifiable();

            // Act
            var result = await controller.Update(1, new RoleViewModel());

            // Assert
            mockRoleManager.Verify();
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<IdentityError>>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Delete_WithRoleId_ReturnsOkResult()
        {
            // Arrange
            var mockRoleManager = GetRoleManagerMock();
            var controller = new RoleController(mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationRole()).Verifiable();
            mockRoleManager.Setup(roleManager => roleManager.DeleteAsync(It.IsAny<ApplicationRole>())).ReturnsAsync(IdentityResult.Success).Verifiable();

            // Act
            var result = await controller.Delete(1);

            // Assert
            mockRoleManager.Verify();
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddPermission_WithPermissionId_ReturnsOkResult()
        {
            // Arrange
            var appRole = new ApplicationRole();
            var mockRoleManager = GetRoleManagerMock();
            var controller = new RoleController(mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(appRole).Verifiable();
            mockRoleManager.Setup(roleManager => roleManager.AddClaimAsync(
                It.Is<ApplicationRole>(role => role.Equals(appRole)),
                It.Is<Claim>(claim => claim.Type.Equals("Permission") && claim.Value.Equals("1"))))
                           .ReturnsAsync(IdentityResult.Success)
                           .Verifiable();

            // Act
            var result = await controller.AddPermission(1, 1);

            // Assert
            mockRoleManager.Verify();
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RemovePermission_WithPermissionId_ReturnsOkResult()
        {
            // Arrange
            var appRole = new ApplicationRole();
            var mockRoleManager = GetRoleManagerMock();
            var controller = new RoleController(mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(appRole).Verifiable();
            mockRoleManager.Setup(roleManager => roleManager.RemoveClaimAsync(
                It.Is<ApplicationRole>(role => role.Equals(appRole)),
                It.Is<Claim>(claim => claim.Type.Equals("Permission") && claim.Value.Equals("1"))))
                           .ReturnsAsync(IdentityResult.Success)
                           .Verifiable();

            // Act
            var result = await controller.RemovePermission(1, 1);

            // Assert
            mockRoleManager.Verify();
            Assert.IsType<OkResult>(result);
        }

        private static IEnumerable<ApplicationRole> GetRoles()
        {
            return new List<ApplicationRole>()
            {
                new ApplicationRole()
                {
                    Id = 1,
                    Name = "Admin",
                    ConcurrencyStamp = "AdminStamp"
                },
                new ApplicationRole()
                {
                    Id = 2,
                    Name = "User",
                    ConcurrencyStamp = "UserStamp"
                },
                new ApplicationRole()
                {
                    Id = 3,
                    Name = "Demo",
                    ConcurrencyStamp = "DemoStamp"
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
