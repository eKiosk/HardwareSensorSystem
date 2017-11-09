using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardwareSensorSystem.Security.Controllers
{
    public class PermissionController : Controller
    {
        public async Task<IActionResult> GetAll()
        {
            var permissions = new List<PermissionViewModel>();
            return Ok(permissions);
        }
    }
}
