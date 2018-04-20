using HardwareSensorSystem.SensorTechnology.Models;
using HardwareSensorSystem.SensorTechnology.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareSensorSystem.SensorTechnology.Controllers
{
    [Route("api/devices")]
    public class DeviceController : Controller
    {
        private readonly SensorTechnologyDbContext _context;

        public DeviceController(SensorTechnologyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var devices = await _context.Devices.Select(d => d.ToViewModel()).ToListAsync();
            return Ok(devices);
        }

        [HttpGet("{deviceId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetById([FromRoute]int deviceId)
        {
            var device = await _context.Devices.SingleOrDefaultAsync(d => d.Id.Equals(deviceId));
            return Ok(device.ToViewModel());
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody]DeviceViewModel device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbDevice = new Device()
            {
                Name = device.Name,
                NormalizedName = device.Name.Normalize().ToUpperInvariant()
            };

            _context.Devices.Add(dbDevice);
            await _context.SaveChangesAsync();

            return Ok(dbDevice.ToViewModel());
        }

        [HttpPut("{deviceId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Update([FromRoute]int deviceId, [FromBody]DeviceViewModel device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbDevice = new Device()
            {
                Id = deviceId,
                Name = device.Name,
                NormalizedName = device.Name.Normalize().ToUpperInvariant()
            };

            _context.Devices.Update(dbDevice);
            await _context.SaveChangesAsync();

            return Ok(dbDevice.ToViewModel());
        }

        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> Delete([FromRoute]int deviceId)
        {
            var dbDevice = _context.Devices.SingleOrDefault(d => d.Id.Equals(deviceId));
            _context.Devices.Remove(dbDevice);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
