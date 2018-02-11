using System;

namespace HardwareSensorSystem.SensorTechnology.Models
{
    public class MeasuredValuePerDay
    {
        public int SensorId { get; set; }

        public int QuantityId { get; set; }

        public DateTime Date { get; set; }

        public float MinValue { get; set; }

        public float MaxValue { get; set; }
    }
}
