using HardwareSensorSystem.SensorTechnology.Controllers;
using HardwareSensorSystem.SensorTechnology.Models;
using HardwareSensorSystem.SensorTechnology.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HardwareSensorSystem.SensorTechnology.Tests
{
    public class DeviceControllerTests
    {
        [Fact]
        public async Task GetAll_WhenCalled_ReturnsCollectionOfDevices()
        {
            // Arrange
            var testDevices = GetDevices();
            var dbContext = Setup.GetDbContext();
            dbContext.Devices.AddRange(testDevices);
            dbContext.SaveChanges();
            var controller = new DeviceController(dbContext);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var devices = Assert.IsAssignableFrom<IEnumerable<DeviceViewModel>>(okObjectResult.Value);
            Assert.All(testDevices, testDevice =>
            {
                var device = devices.SingleOrDefault(d => d.Id == testDevice.Id);
                Assert.NotNull(device);
                Assert.Equal(testDevice.Name, device.Name);
            });
        }

        [Fact]
        public async Task GetById_WithDeviceId_ReturnsDevice()
        {
            // Arrange
            var testDevice = new Device()
            {
                Id = 10,
                Name = "DeviceName"
            };
            var dbContext = Setup.GetDbContext();
            dbContext.Devices.Add(testDevice);
            dbContext.SaveChanges();
            var controller = new DeviceController(dbContext);

            // Act
            var result = await controller.GetById(testDevice.Id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var device = Assert.IsAssignableFrom<DeviceViewModel>(okObjectResult.Value);
            Assert.Equal(testDevice.Id, device.Id);
            Assert.Equal(testDevice.Name, device.Name);
        }

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

        private static IEnumerable<Device> GetDevices()
        {
            return new List<Device>()
            {
                new Device()
                {
                    Id=1,
                    Name="Device_1"
                },
                new Device()
                {
                    Id=2,
                    Name="Device_2"
                },
                new Device()
                {
                    Id=3,
                    Name="Device_3"
                }
            };
        }
    }
}
