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
    public class SensorControllerTests
    {
        [Fact]
        public async Task GetAll_WithDeviceId_ReturnsCollectionOfSensors()
        {
            // Arrange
            var testSensors = GetSensors();
            var dbContext = Setup.GetDbContext();
            dbContext.Sensors.AddRange(testSensors);
            dbContext.SaveChanges();
            var controller = new SensorController(dbContext);

            // Act
            var result = await controller.GetAllInDevice(1);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var sensors = Assert.IsAssignableFrom<IEnumerable<SensorViewModel>>(okObjectResult.Value);
            Assert.All(testSensors, testSensor =>
            {
                var sensor = sensors.SingleOrDefault(s => s.Id == testSensor.Id);
                Assert.NotNull(sensor);
                Assert.Equal(testSensor.DeviceId, sensor.DeviceId);
                Assert.Equal(testSensor.Name, sensor.Name);
            });
        }

        [Fact]
        public async Task GetById_WithSensorId_ReturnsSensorWithProperties()
        {
            // Arrange
            var testSensor = new Sensor()
            {
                Id = 1,
                DeviceId = 1,
                Name = "SensorName"
            };
            var testSensorProperties = new List<SensorProperty>()
            {
                new SensorProperty()
                {
                    Id = 1,
                    SensorId = 1,
                    Name = "Property_1",
                    Value = "JH6GTNH8"
                },
                new SensorProperty()
                {
                    Id = 2,
                    SensorId = 1,
                    Name = "Property_2",
                    Value = "OUH65BRF8"
                }
            };
            var dbContext = Setup.GetDbContext();
            dbContext.Sensors.Add(testSensor);
            dbContext.SensorProperties.AddRange(testSensorProperties);
            dbContext.SaveChanges();
            var controller = new SensorController(dbContext);

            // Act
            var result = await controller.GetById(testSensor.Id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var sensor = Assert.IsAssignableFrom<SensorViewModel>(okObjectResult.Value);
            Assert.Equal(testSensor.Name, sensor.Name);
            Assert.All(testSensorProperties, testSensorProperty =>
            {
                var sensorProperty = sensor.Properties.SingleOrDefault(sp => sp.Name.Equals(testSensorProperty.Name));
                Assert.NotNull(sensorProperty);
                Assert.Equal(testSensorProperty.Value, sensorProperty.Value);
            });
        }

        [Fact]
        public async Task GetById_WithInvalidSensorId_ReturnsNotFound()
        {
            // Arrange
            var dbContext = Setup.GetDbContext();
            var controller = new SensorController(dbContext);

            // Act
            var result = await controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_WithValidSensor_ReturnsCreatedSensor()
        {
            // Arrange
            var testSensor = new SensorViewModel()
            {
                DeviceId = 1,
                Name = "TestSensor",
                Properties = new List<SensorPropertyViewModel>()
                {
                    new SensorPropertyViewModel()
                    {
                        Name = "Property",
                        Value = "JSDH6H"
                    }
                }
            };
            var dbContext = Setup.GetDbContext();
            var controller = new SensorController(dbContext);

            // Act
            var result = await controller.Create(testSensor);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var sensor = Assert.IsAssignableFrom<SensorViewModel>(okObjectResult.Value);
            Assert.Equal(1, sensor.Id);
            Assert.Equal(testSensor.Name, sensor.Name);
            Assert.All(testSensor.Properties, testSensorProperty =>
            {
                var sensorProperty = sensor.Properties.SingleOrDefault(sp => sp.Name.Equals(testSensorProperty.Name));
                Assert.NotNull(sensorProperty);
                Assert.Equal(testSensorProperty.Value, sensorProperty.Value);
            });
        }

        [Fact]
        public async Task Create_WithInvalidSensor_ReturnsModelError()
        {
            // Arrange
            var dbContext = Setup.GetDbContext();
            var controller = new SensorController(dbContext);
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Create(new SensorViewModel());

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
            Assert.Empty(dbContext.Sensors);
        }

        [Fact]
        public async Task Update_WithValidSensor_ReturnsUpdatedSensor()
        {
            // Arrange
            var testSensor = new SensorViewModel()
            {
                DeviceId = 1,
                Name = "TestSensor",
                Properties = new List<SensorPropertyViewModel>()
                {
                    new SensorPropertyViewModel()
                    {
                        Name = "Property",
                        Value = "JSDH6H"
                    }
                }
            };
            var dbContext = Setup.GetDbContext();
            var sensorEntry = dbContext.Sensors.Add(new Sensor()
            {
                Id = 10,
                DeviceId = 1,
                Name = "SensorName"
            });
            var sensorProperty1 = dbContext.SensorProperties.Add(new SensorProperty()
            {
                SensorId = 10,
                Name = "Property",
                Value = "KDU8J"
            });
            var sensorProperty2 = dbContext.SensorProperties.Add(new SensorProperty()
            {
                SensorId = 10,
                Name = "Key",
                Value = "12DE83"
            });
            await dbContext.SaveChangesAsync();
            sensorEntry.State = EntityState.Detached;
            sensorProperty1.State = EntityState.Detached;
            sensorProperty2.State = EntityState.Detached;
            var controller = new SensorController(dbContext);

            // Act
            var result = await controller.Update(10, testSensor);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var sensor = Assert.IsAssignableFrom<SensorViewModel>(okObjectResult.Value);
            Assert.Equal(10, sensor.Id);
            Assert.Equal(testSensor.Name, sensor.Name);
            Assert.All(testSensor.Properties, testSensorProperty =>
            {
                var sensorProperty = sensor.Properties.SingleOrDefault(sp => sp.Name.Equals(testSensorProperty.Name));
                Assert.NotNull(sensorProperty);
                Assert.Equal(testSensorProperty.Value, sensorProperty.Value);
            });
        }

        [Fact]
        public async Task Update_WithInvalidSensor_ReturnsModelError()
        {
            // Arrange
            var dbContext = Setup.GetDbContext();
            var controller = new SensorController(dbContext);
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Update(1, new SensorViewModel());

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Delete_WithSensorId_ReturnsOkResult()
        {
            // Arrange
            var dbContext = Setup.GetDbContext();
            var sensorEntry = dbContext.Sensors.Add(new Sensor()
            {
                Id = 10,
                DeviceId = 1,
                Name = "SensorName",
            });
            await dbContext.SaveChangesAsync();
            sensorEntry.State = EntityState.Detached;
            var controller = new SensorController(dbContext);

            // Act
            var result = await controller.Delete(10);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Empty(dbContext.Sensors);
        }

        private static IEnumerable<Sensor> GetSensors()
        {
            return new List<Sensor>()
            {
                new Sensor()
                {
                    Id = 1,
                    DeviceId = 1,
                    Name = "Sensor_1"
                },
                new Sensor()
                {
                    Id = 2,
                    DeviceId = 1,
                    Name = "Sensor_2"
                },
                new Sensor()
                {
                    Id = 3,
                    DeviceId = 1,
                    Name = "Sensor_3"
                }
            };
        }
    }
}
