using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [AllowAnonymous]
        [HttpPost("api/devices/{deviceId}/log")]
        public async Task<IActionResult> Log([FromRoute]int deviceId)
        {
            XDocument file = XDocument.Load(HttpContext.Request.Body);

            var romIdName = XName.Get("ROMId", file.Root.Name.NamespaceName);
            var dataSensors = file.Root.Elements().Select(sensor => new
            {
                SerialNumber = sensor.Element(romIdName).Value,
                Data = sensor.Elements().Where(element => element != sensor.Element(romIdName))
            });

            return Ok();
        }
    }
}
