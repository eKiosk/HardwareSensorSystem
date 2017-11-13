using HardwareSensorSystem.Security.Controllers;
using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        public async Task GetAll_WhenCalled_ReturnsCollectionOfUsersFromUserManager()
        {
            // Arrange
            var testUsers = GetUsers();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new List<IUserValidator<ApplicationUser>>(),
                new List<IPasswordValidator<ApplicationUser>>(),
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
            mockUserManager.Setup(userManager => userManager.Users).Returns(testUsers.AsQueryable());
            var controller = new UserController(mockUserManager.Object);

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
