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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbSensor = new Sensor()
            {
                DeviceId = sensor.DeviceId,
                Name = sensor.Name
            };

            _context.Sensors.Add(dbSensor);

            var sensorProperties = sensor.Properties.Select(sp => new SensorProperty()
            {
                SensorId = dbSensor.Id,
                Name = sp.Name,
                Value = sp.Value
            });
            _context.SensorProperties.AddRange(sensorProperties);

            await _context.SaveChangesAsync();

            var sensorModel = dbSensor.ToViewModel();
            sensorModel.Properties = sensorProperties.Select(sp => sp.ToViewModel());

            return Ok(sensorModel);
        }

        [HttpPut("{sensorId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Update([FromRoute]int sensorId, [FromBody]SensorViewModel sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbSensor = new Sensor()
            {
                Id = sensorId,
                DeviceId = sensor.DeviceId,
                Name = sensor.Name
            };

            _context.Sensors.Update(dbSensor);

            var sensorProperties = sensor.Properties.Select(sp => new SensorProperty()
            {
                SensorId = dbSensor.Id,
                Name = sp.Name,
                Value = sp.Value
            });
            _context.SensorProperties.RemoveRange(_context.SensorProperties.Where(sp => sp.SensorId.Equals(sensorId)));
            _context.SensorProperties.AddRange(sensorProperties);

            await _context.SaveChangesAsync();

            var sensorModel = dbSensor.ToViewModel();
            sensorModel.Properties = sensorProperties.Select(sp => sp.ToViewModel());

            return Ok(sensorModel);
        }

        [HttpDelete("{sensorId}")]
        public async Task<IActionResult> Delete([FromRoute]int sensorId)
        {
            var dbSensor = _context.Sensors.SingleOrDefault(d => d.Id.Equals(sensorId));
            _context.Sensors.Remove(dbSensor);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
