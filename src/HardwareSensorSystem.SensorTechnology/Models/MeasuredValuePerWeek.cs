namespace HardwareSensorSystem.SensorTechnology.Models
{
    public class MeasuredValuePerWeek
    {
        public int SensorId { get; set; }

        public int QuantityId { get; set; }

        public short Year { get; set; }

        public byte Week { get; set; }

        public float MinValue { get; set; }

        public float MaxValue { get; set; }
    }
}
