using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareSensorSystem.Security.Controllers
{
    public class PermissionController : Controller
    {
        private ApplicationDbContext _context;

        public PermissionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<IActionResult> GetAll()
        {
            var permissions = _context.Permissions.Select(p => p.ToViewModel());
            return Task.FromResult<IActionResult>(Ok(permissions));
        }
    }
}
