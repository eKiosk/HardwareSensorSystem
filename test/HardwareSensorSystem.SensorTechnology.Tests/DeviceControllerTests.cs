using HardwareSensorSystem.SensorTechnology.Controllers;
using HardwareSensorSystem.SensorTechnology.Models;
using HardwareSensorSystem.SensorTechnology.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace HardwareSensorSystem.SensorTechnology.Tests
{
    public class DeviceControllerTests
    {
        [Fact]
        public async Task Create_WithValidDevice_ReturnsCreatedDevice()
        {
            // Arrange
            var dbContext = Setup.GetDbContext();
            var controller = new DeviceController(dbContext);

            // Act
            var result = await controller.Create(new DeviceViewModel() { Name = "TestDevice" });

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var device = Assert.IsAssignableFrom<DeviceViewModel>(okObjectResult.Value);
            Assert.Equal(1, device.Id);
            Assert.Equal("TestDevice", device.Name);
            var dbDevice = Assert.Single(dbContext.Devices);
            Assert.Equal("TESTDEVICE", dbDevice.NormalizedName);
        }

        [Fact]
        public async Task Create_WithInvalidDevice_ReturnsModelError()
        {
            // Arrange
            var dbContext = Setup.GetDbContext();
            var controller = new DeviceController(dbContext);
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Create(new DeviceViewModel());

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
            Assert.Empty(dbContext.Devices);
        }

        [Fact]
        public async Task Update_WithValidDevice_ReturnsUpdatedDevice()
        {
            // Arrange
            var dbContext = Setup.GetDbContext();
            var deviceEntry = dbContext.Devices.Add(new Device()
            {
                Id = 10,
                Name = "DeviceName",
                NormalizedName = "DEVICENAME"
            });
            await dbContext.SaveChangesAsync();
            deviceEntry.State = EntityState.Detached;
            var controller = new DeviceController(dbContext);

            // Act
            var result = await controller.Update(10, new DeviceViewModel() { Name = "TestDevice" });

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var device = Assert.IsAssignableFrom<DeviceViewModel>(okObjectResult.Value);
            Assert.Equal(10, device.Id);
            Assert.Equal("TestDevice", device.Name);
            var dbDevice = Assert.Single(dbContext.Devices);
            Assert.Equal("TESTDEVICE", dbDevice.NormalizedName);
        }

        [Fact]
        public async Task Update_WithInvalidDevice_ReturnsModelError()
        {
            // Arrange
            var dbContext = Setup.GetDbContext();
            var controller = new DeviceController(dbContext);
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Update(1, new DeviceViewModel());

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
            Assert.Empty(dbContext.Devices);
        }

        [Fact]
        public async Task Delete_WithDeviceId_ReturnsOkResult()
        {
            // Arrange
            var dbContext = Setup.GetDbContext();
            var deviceEntry = dbContext.Devices.Add(new Device()
            {
                Id = 10,
                Name = "DeviceName",
                NormalizedName = "DEVICENAME"
            });
            await dbContext.SaveChangesAsync();
            deviceEntry.State = EntityState.Detached;
            var controller = new DeviceController(dbContext);

            // Act
            var result = await controller.Delete(10);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Empty(dbContext.Devices);
        }
    }
}
