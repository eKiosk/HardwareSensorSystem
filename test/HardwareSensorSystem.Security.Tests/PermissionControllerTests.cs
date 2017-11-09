using HardwareSensorSystem.Security.Controllers;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HardwareSensorSystem.Security.Tests
{
    public class PermissionControllerTests
    {
        [Fact]
        public async Task GetAll_Call_ReturnCollectionOfPermissions()
        {
            // Arrange
            var controller = new PermissionController();

            // Act
            var result = await controller.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<PermissionViewModel>>(okObjectResult.Value);
        }
    }
}
