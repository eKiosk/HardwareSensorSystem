using HardwareSensorSystem.Security.Controllers;
using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HardwareSensorSystem.Security.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetAll_WhenCalled_ReturnsCollectionOfUsersFromUserManager()
        {
            // Arrange
            var testUsers = GetUsers();
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var dbContext = Setup.GetDbContext();
            dbContext.Users.AddRange(testUsers);
            dbContext.SaveChanges();
            mockUserManager.Setup(userManager => userManager.Users).Returns(dbContext.Users);
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsAssignableFrom<IEnumerable<UserViewModel>>(okObjectResult.Value);
            Assert.All(testUsers, testUser =>
            {
                var user = users.SingleOrDefault(u => u.Id == testUser.Id);
                Assert.NotNull(user);
                Assert.Equal(testUser.UserName, user.UserName);
                Assert.Equal(testUser.Email, user.Email);
            });
        }

        [Fact]
        public async Task Create_WithValidUser_ReturnCreatedUser()
        {
            // Arrange
            var userName = "UserName";
            var userEmail = "user@example.org";
            var userConcurrencyStamp = "UserStamp";
            var testRole = new ApplicationRole()
            {
                Id = 10,
                Name = "RoleName",
                ConcurrencyStamp = "RoleStamp"
            };
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.FindByIdAsync(
                    It.Is<string>(roleId => roleId.Equals(testRole.Id.ToString()))
                )).ReturnsAsync(testRole);
            mockUserManager.Setup(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser appUser, string appUserPassword) =>
               {
                   appUser.Id = 1;
                   appUser.ConcurrencyStamp = userConcurrencyStamp;
                   return IdentityResult.Success;
               }).Verifiable();
            mockUserManager.Setup(userManager => userManager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    It.Is<string>(roleName => roleName.Equals(testRole.Name))
                )).ReturnsAsync(IdentityResult.Success).Verifiable();

            // Act
            var result = await controller.Create(new UserCreateViewModel()
            {
                UserName = userName,
                Email = userEmail,
                Password = "12345678",
                RoleId = testRole.Id
            });

            // Assert
            mockUserManager.Verify();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsAssignableFrom<UserViewModel>(okObjectResult.Value);
            Assert.Equal(1, user.Id);
            Assert.Equal(userName, user.UserName);
            Assert.Equal(userEmail, user.Email);
            Assert.Equal(userConcurrencyStamp, user.ConcurrencyStamp);
        }

        [Fact]
        public async Task Create_WithInvalidUser_ReturnsModelError()
        {
            // Arrange
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            controller.ModelState.AddModelError("UserName", "Required");

            // Act
            var result = await controller.Create(new UserCreateViewModel());

            // Assert
            mockUserManager.Verify(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never());
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Create_WithInvalidRoleId_ReturnsModelError()
        {
            // Arrange
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationRole)null);

            // Act
            var result = await controller.Create(new UserCreateViewModel() { RoleId = 1 });

            // Assert
            mockUserManager.Verify(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never());
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Create_WithUnprocessableUser_ReturnsCollectionOfIdentityErrors()
        {
            // Arrange
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationRole());
            mockUserManager.Setup(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed()).Verifiable();

            // Act
            var result = await controller.Create(new UserCreateViewModel());

            // Assert
            mockUserManager.Verify();
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<IdentityError>>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Update_WithValidUser_ReturnsUpdatedUser()
        {
            // Arrange
            var oldRoleName = "OldRoleName";
            var newRole = new ApplicationRole()
            {
                Id = 10,
                Name = "NewRoleName",
                ConcurrencyStamp = "NewRoleStamp"
            };
            var userId = 1;
            var userConcurrencyStampOld = "OldUserStamp";
            var userConcurrencyStampNew = "NewUserStamp";
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.FindByIdAsync(
                    It.Is<string>(roleId => roleId.Equals(newRole.Id.ToString()))
                )).ReturnsAsync(newRole);
            mockUserManager.Setup(userManager => userManager.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync((ApplicationUser appUser) =>
                {
                    appUser.ConcurrencyStamp = userConcurrencyStampNew;
                    return IdentityResult.Success;
                }).Verifiable();
            mockUserManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string>() { oldRoleName }).Verifiable();
            mockUserManager.Setup(userManager => userManager.RemoveFromRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    It.Is<string>(roleName => roleName.Equals(oldRoleName))
                )).ReturnsAsync(IdentityResult.Success).Verifiable();
            mockUserManager.Setup(userManager => userManager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    It.Is<string>(roleName => roleName.Equals(newRole.Name))
                )).ReturnsAsync(IdentityResult.Success).Verifiable();
            mockUserManager.Setup(userManager => userManager.RemovePasswordAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success).Verifiable();
            mockUserManager.Setup(userManager => userManager.AddPasswordAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()
                )).ReturnsAsync(IdentityResult.Success).Verifiable();

            // Act
            var result = await controller.Update(userId, new UserUpdateViewModel()
            {
                Password = "12345678",
                RoleId = newRole.Id,
                ConcurrencyStamp = userConcurrencyStampOld
            });

            // Assert
            mockUserManager.Verify();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsAssignableFrom<UserViewModel>(okObjectResult.Value);
            Assert.Equal(userConcurrencyStampNew, user.ConcurrencyStamp);
        }

        [Fact]
        public async Task Update_WithoutPasswordChange_ReturnsUpdatedUser()
        {
            // Arrange
            var oldRoleName = "OldRoleName";
            var newRole = new ApplicationRole()
            {
                Id = 10,
                Name = "NewRoleName",
                ConcurrencyStamp = "NewRoleStamp"
            };
            var userId = 1;
            var userConcurrencyStampOld = "OldUserStamp";
            var userConcurrencyStampNew = "NewUserStamp";
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.FindByIdAsync(
                    It.Is<string>(roleId => roleId.Equals(newRole.Id.ToString()))
                )).ReturnsAsync(newRole);
            mockUserManager.Setup(userManager => userManager.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync((ApplicationUser appUser) =>
                {
                    appUser.ConcurrencyStamp = userConcurrencyStampNew;
                    return IdentityResult.Success;
                }).Verifiable();
            mockUserManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string>() { oldRoleName }).Verifiable();
            mockUserManager.Setup(userManager => userManager.RemoveFromRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    It.Is<string>(roleName => roleName.Equals(oldRoleName))
                )).ReturnsAsync(IdentityResult.Success).Verifiable();
            mockUserManager.Setup(userManager => userManager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    It.Is<string>(roleName => roleName.Equals(newRole.Name))
                )).ReturnsAsync(IdentityResult.Success).Verifiable();

            // Act
            var result = await controller.Update(userId, new UserUpdateViewModel()
            {
                RoleId = newRole.Id,
                ConcurrencyStamp = userConcurrencyStampOld
            });

            // Assert
            mockUserManager.Verify();
            mockUserManager.Verify(userManager => userManager.RemovePasswordAsync(It.IsAny<ApplicationUser>()), Times.Never());
            mockUserManager.Verify(userManager => userManager.AddPasswordAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()
                ), Times.Never());
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsAssignableFrom<UserViewModel>(okObjectResult.Value);
            Assert.Equal(userConcurrencyStampNew, user.ConcurrencyStamp);
        }

        private static IEnumerable<ApplicationUser> GetUsers()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id = 1,
                    UserName = "Administrator",
                    Email = "admin@example.com"
                },
                new ApplicationUser()
                {
                    Id = 2,
                    UserName = "Anna",
                    Email = "anna@example.com"
                },
                new ApplicationUser()
                {
                    Id = 3,
                    UserName = "Bob",
                    Email = "bob@example.com"
                }
            };
        }
    }
}
