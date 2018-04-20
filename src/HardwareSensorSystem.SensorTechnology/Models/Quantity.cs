using System.Collections.Generic;

namespace HardwareSensorSystem.SensorTechnology.Models
{
    public class Quantity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }
    }

    public static class Quantities
    {
        private static readonly ICollection<Quantity> Values = new List<Quantity>
        {
            new Quantity
            {
                Id = 1,
                Name = "Temperature",
                Unit = "°C"
            },
            new Quantity
            {
                Id = 2,
                Name = "Relative humidity",
                Unit = "%"
            },
            new Quantity
            {
                Id = 3,
                Name = "Electric current",
                Unit = "A"
            }
        };

        public static ICollection<Quantity> Get()
        {
            return Values;
        }
    }
}
