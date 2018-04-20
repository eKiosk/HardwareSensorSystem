using HardwareSensorSystem.SensorTechnology.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HardwareSensorSystem.SensorTechnology.Controllers
{
    public class OwServerEnet2LogController : Controller
    {
        private readonly SensorTechnologyDbContext _context;

        public OwServerEnet2LogController(SensorTechnologyDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("api/devices/{deviceId}/log/ow-server-enet")]
        public async Task<IActionResult> Log([FromRoute]int deviceId)
        {
            XDocument file = XDocument.Load(HttpContext.Request.Body);

            var romIdName = XName.Get("ROMId", file.Root.Name.NamespaceName);
            var dataSensors = file.Root.Elements().Select(sensor => new
            {
                SerialNumber = sensor.Element(romIdName).Value,
                Data = sensor.Elements().Where(element => element != sensor.Element(romIdName))
            });

            var groupedProperties = await _context.Sensors.Where(sensor => sensor.DeviceId.Equals(deviceId))
                .Join(_context.SensorProperties
                          .Where(property => property.Name.Equals("SENSOR_ID"))
                          .Where(property => dataSensors.Any(e => e.SerialNumber.Equals(property.Value))),
                      sensor => sensor.Id,
                      property => property.SensorId,
                      (sensor, _) => sensor)
                .GroupJoin(_context.SensorProperties,
                           sensor => sensor.Id,
                           property => property.SensorId,
                           (_, properties) => properties)
                .ToListAsync();

            foreach (var properties in groupedProperties)
            {
                var serialNumber = properties.Single(property => property.Name.Equals("SENSOR_ID")).Value;

                try
                {
                    var data = dataSensors.Single(e => e.SerialNumber.Equals(serialNumber)).Data;
                }
                catch
                {

                }
            }

            return Ok();
        }
    }
}
