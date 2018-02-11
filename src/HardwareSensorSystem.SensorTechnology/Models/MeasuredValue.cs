using System;

namespace HardwareSensorSystem.SensorTechnology.Models
{
    public class MeasuredValue
    {
        public int SensorId { get; set; }

        public int QuantityId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public float Value { get; set; }
    }
}
