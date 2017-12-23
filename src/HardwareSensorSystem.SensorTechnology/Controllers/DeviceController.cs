using HardwareSensorSystem.SensorTechnology.Models;
using HardwareSensorSystem.SensorTechnology.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareSensorSystem.SensorTechnology.Controllers
{
    public class DeviceController : Controller
    {
        private SensorTechnologyDbContext _context;

        public DeviceController(SensorTechnologyDbContext context)
        {
            _context = context;
        }

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

        public async Task<IActionResult> Delete([FromRoute]int deviceId)
        {
            var dbDevice = _context.Devices.SingleOrDefault(d => d.Id.Equals(deviceId));
            _context.Devices.Remove(dbDevice);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
