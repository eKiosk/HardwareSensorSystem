using HardwareSensorSystem.SensorTechnology.Models;
using HardwareSensorSystem.SensorTechnology.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareSensorSystem.SensorTechnology.Controllers
{
    [Route("api/sensors")]
    public class SensorController : Controller
    {
        private SensorTechnologyDbContext _context;

        public SensorController(SensorTechnologyDbContext context)
        {
            _context = context;
        }

        [HttpGet("~/devices/{deviceId}/sensors")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllInDevice([FromRoute]int deviceId)
        {
            var sensors = await _context.Sensors.Where(s => s.DeviceId.Equals(deviceId)).Select(s => s.ToViewModel()).ToListAsync();
            return Ok(sensors);
        }

        [HttpGet("{sensorId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetById([FromRoute]int sensorId)
        {
            var sensor = await _context.Sensors.SingleOrDefaultAsync(s => s.Id.Equals(sensorId));
            if (sensor is null)
            {
                return NotFound();
            }

            var sensorProperties = _context.SensorProperties.Where(sp => sp.SensorId.Equals(sensorId));

            var sensorModel = sensor.ToViewModel();
            sensorModel.Properties = sensorProperties.Select(sp => sp.ToViewModel());

            return Ok(sensorModel);
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody]SensorViewModel sensor)
        {
            var dbSensor = new Sensor()
            {
                DeviceId = sensor.DeviceId,
                Name = sensor.Name
            };

            _context.Sensors.Add(dbSensor);
            await _context.SaveChangesAsync();

            foreach (var property in sensor.Properties)
            {
                var dbProperty = new SensorProperty()
                {
                    SensorId = dbSensor.Id,
                    Name = property.Name,
                    Value = property.Value
                };
                _context.SensorProperties.Add(dbProperty);
                await _context.SaveChangesAsync();
            }

            var sensorProperties = _context.SensorProperties.Where(sp => sp.SensorId.Equals(dbSensor.Id));
            var sensorModel = dbSensor.ToViewModel();
            sensorModel.Properties = sensorProperties.Select(sp => sp.ToViewModel());

            return Ok(sensorModel);
        }
    }
}
