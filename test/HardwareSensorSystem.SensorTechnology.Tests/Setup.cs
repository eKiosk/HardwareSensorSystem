using HardwareSensorSystem.SensorTechnology.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace HardwareSensorSystem.SensorTechnology.Tests
{
    public static class Setup
    {
        public static SensorTechnologyDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<SensorTechnologyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new SensorTechnologyDbContext(options);
        }
    }
}
