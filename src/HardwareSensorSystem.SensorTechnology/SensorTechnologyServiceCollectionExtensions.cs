using HardwareSensorSystem.SensorTechnology.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class SensorTechnologyServiceCollectionExtensions
    {
        public static void AddSensorTechnology(this IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptionsAction)
        {
            services.AddDbContext<SensorTechnologyDbContext>(dbContextOptionsAction);
        }
    }
}
