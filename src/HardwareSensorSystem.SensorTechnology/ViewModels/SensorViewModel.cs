using HardwareSensorSystem.SensorTechnology.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HardwareSensorSystem.SensorTechnology.ViewModels
{
    public class SensorViewModel
    {
        public int? Id { get; set; }

        [Required]
        public int DeviceId { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<SensorPropertyViewModel> Properties { get; set; }
    }

    public static class SensorViewModelExtensions
    {
        /// <summary>
        /// Convert a <see cref="Sensor"/> to its view model.
        /// </summary>
        /// <param name="appSensor">The sensor.</param>
        /// <returns>A view model of the sensor.</returns>
        public static SensorViewModel ToViewModel(this Sensor appSensor)
        {
            return new SensorViewModel()
            {
                Id = appSensor.Id,
                DeviceId = appSensor.DeviceId,
                Name = appSensor.Name
            };
        }
    }
}
