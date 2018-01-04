using HardwareSensorSystem.SensorTechnology.Models;

namespace HardwareSensorSystem.SensorTechnology.ViewModels
{
    public class SensorPropertyViewModel
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public static class SensorPropertyViewModelExtensions
    {
        /// <summary>
        /// Convert a <see cref="SensorProperty"/> to its view model.
        /// </summary>
        /// <param name="appSensorProperty">The sensor property.</param>
        /// <returns>A view model of the sensor property.</returns>
        public static SensorPropertyViewModel ToViewModel(this SensorProperty appSensorProperty)
        {
            return new SensorPropertyViewModel()
            {
                Name = appSensorProperty.Name,
                Value = appSensorProperty.Value
            };
        }
    }
}
