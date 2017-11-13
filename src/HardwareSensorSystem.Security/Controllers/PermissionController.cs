using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> GetAll()
        {
            var permissions = await _context.Permissions.Select(p => p.ToViewModel()).ToListAsync();
            return Ok(permissions);
        }
    }
}
