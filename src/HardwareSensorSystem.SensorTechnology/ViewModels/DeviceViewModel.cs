using HardwareSensorSystem.SensorTechnology.Models;
using System.ComponentModel.DataAnnotations;

namespace HardwareSensorSystem.SensorTechnology.ViewModels
{
    /// <summary>
    /// View model of device.
    /// </summary>
    public class DeviceViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }
    }

    public static class DeviceViewModelExtensions
    {
        /// <summary>
        /// Convert a <see cref="Device"/> to its view model.
        /// </summary>
        /// <param name="appDevice">The device.</param>
        /// <returns>A view model of the device.</returns>
        public static DeviceViewModel ToViewModel(this Device appDevice)
        {
            return new DeviceViewModel()
            {
                Id = appDevice.Id,
                Name = appDevice.Name
            };
        }
    }
}
