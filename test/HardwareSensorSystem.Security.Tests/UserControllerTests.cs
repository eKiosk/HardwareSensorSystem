using HardwareSensorSystem.Security.Controllers;
using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HardwareSensorSystem.Security.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetAllInRole_WithRoleId_ReturnsCollectionOfUsers()
        {
            // Arrange
            var testRole = new ApplicationRole()
            {
                Id = 10,
                Name = "RoleName",
                ConcurrencyStamp = "RoleStamp"
            };
            var testUsers = GetUsers();
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.FindByIdAsync(
                    It.Is<string>(roleId => roleId.Equals(testRole.Id.ToString()))
                )).ReturnsAsync(testRole);
            mockUserManager.Setup(userManager => userManager.GetUsersInRoleAsync(It.Is<string>(roleName => roleName.Equals(testRole.Name))))
                .ReturnsAsync(testUsers.ToList());

            // Act
            var result = await controller.GetAllInRole(testRole.Id);

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
        public async Task GetById_WithUserId_ReturnsUser()
        {
            // Arrange
            var testUser = new ApplicationUser()
            {
                Id = 10,
                UserName = "UserName",
                Email = "user@example.com",
                ConcurrencyStamp = "UserStamp"
            };
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockUserManager.Setup(userManager => userManager.FindByIdAsync(
                    It.Is<string>(userId => userId.Equals(testUser.Id.ToString()))
                )).ReturnsAsync(testUser);

            // Act
            var result = await controller.GetById(testUser.Id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsAssignableFrom<UserViewModel>(okObjectResult.Value);
            Assert.Equal(testUser.Id, user.Id);
            Assert.Equal(testUser.UserName, user.UserName);
            Assert.Equal(testUser.Email, user.Email);
            Assert.Equal(testUser.ConcurrencyStamp, user.ConcurrencyStamp);
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
        public async Task Update_WithUser_ReturnUpdatedUser()
        {
            // Arrange
            var userStamp = "NewUserStamp";
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockUserManager.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((string userId) =>
              {
                  return new ApplicationUser() { Id = Convert.ToInt32(userId) };

              });
            mockUserManager.Setup(userManager => userManager.UpdateAsync(It.Is<ApplicationUser>(appUser => appUser.Id.Equals(1))))
                .ReturnsAsync((ApplicationUser appUser) =>
                {
                    appUser.ConcurrencyStamp = userStamp;
                    return IdentityResult.Success;
                });

            // Act
            var result = await controller.Update(1, new UserUpdateViewModel()
            {
                ConcurrencyStamp = "OldUserStamp"
            });

            // Assert
            mockUserManager.Verify(userManager => userManager.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()), Times.Never());
            mockUserManager.Verify(userManager => userManager.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never());
            mockUserManager.Verify(userManager => userManager.RemovePasswordAsync(It.IsAny<ApplicationUser>()), Times.Never());
            mockUserManager.Verify(userManager => userManager.AddPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never());
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsAssignableFrom<UserViewModel>(okObjectResult.Value);
            Assert.Equal(userStamp, user.ConcurrencyStamp);
        }

        [Fact]
        public async Task Update_WithPasswordChange_ReturnsUpdatedUser()
        {
            // Arrange
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockUserManager.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((string userId) =>
            {
                return new ApplicationUser() { Id = Convert.ToInt32(userId) };

            });
            mockUserManager.Setup(userManager => userManager.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(userManager => userManager.RemovePasswordAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success).Verifiable();
            mockUserManager.Setup(userManager => userManager.AddPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Verifiable();

            // Act
            var result = await controller.Update(1, new UserUpdateViewModel()
            {
                Password = "12345678"
            });

            // Assert
            mockUserManager.Verify();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<UserViewModel>(okObjectResult.Value);
        }

        [Fact]
        public async Task Update_WithRoleChange_ReturnsUpdatedUser()
        {
            // Arrange
            var testRole = new ApplicationRole()
            {
                Id = 10,
                Name = "TestRole"
            };
            var userRoles = new List<string>() { "OldRoleName" };
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockUserManager.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((string userId) =>
            {
                return new ApplicationUser() { Id = Convert.ToInt32(userId) };

            });
            mockUserManager.Setup(userManager => userManager.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(userRoles.ToList());
            mockUserManager.Setup(userManager => userManager.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync((ApplicationUser user, IEnumerable<string> roles) =>
                {
                    foreach (var role in roles)
                    {
                        userRoles.Remove(role);
                    }
                    return IdentityResult.Success;
                });
            mockUserManager.Setup(userManager => userManager.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser user, string role) =>
                {
                    userRoles.Add(role);
                    return IdentityResult.Success;
                });
            mockRoleManager.Setup(roleManager => roleManager.FindByIdAsync(It.Is<string>(roleId => roleId.Equals(testRole.Id.ToString())))).ReturnsAsync(testRole);

            // Act
            var result = await controller.Update(1, new UserUpdateViewModel()
            {
                RoleId = testRole.Id
            });

            // Assert
            mockUserManager.Verify();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<UserViewModel>(okObjectResult.Value);
            var roleName = Assert.Single(userRoles);
            Assert.Equal(testRole.Name, roleName);
        }

        [Fact]
        public async Task Update_WithInvalidUser_ReturnsModelError()
        {
            // Arrange
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            controller.ModelState.AddModelError("UserName", "Required");

            // Act
            var result = await controller.Update(1, new UserUpdateViewModel());

            // Assert
            mockUserManager.Verify(userManager => userManager.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never());
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Update_WithInvalidRoleId_ReturnsModelError()
        {
            // Arrange
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockRoleManager.Setup(roleManager => roleManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(null as ApplicationRole);

            // Act
            var result = await controller.Update(1, new UserUpdateViewModel() { RoleId = 1 });

            // Assert
            mockUserManager.Verify(userManager => userManager.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never());
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Update_WithUnprocessableUser_ReturnsCollectionOfIdentityErrors()
        {
            // Arrange
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockUserManager.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((string userId) =>
            {
                return new ApplicationUser() { Id = Convert.ToInt32(userId) };

            });
            mockUserManager.Setup(userManager => userManager.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Failed()).Verifiable();

            // Act
            var result = await controller.Update(1, new UserUpdateViewModel());

            // Assert
            mockUserManager.Verify();
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<IdentityError>>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Delete_WithUserId_ReturnsOkResult()
        {
            // Arrange
            var mockUserManager = Setup.GetUserManagerMock();
            var mockRoleManager = Setup.GetRoleManagerMock();
            var controller = new UserController(mockUserManager.Object, mockRoleManager.Object);
            mockUserManager.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser()).Verifiable();
            mockUserManager.Setup(userManager => userManager.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success).Verifiable();

            // Act
            var result = await controller.Delete(1);

            // Assert
            mockUserManager.Verify();
            Assert.IsType<OkResult>(result);
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
