using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareSensorSystem.Security.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<ApplicationRole> _roleManager;

        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public Task<IActionResult> GetAll()
        {
            var roles = _roleManager.Roles.Select(r => r.ToViewModel());
            return Task.FromResult<IActionResult>(Ok(roles));
        }
    }
}
