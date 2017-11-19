using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleManager.Roles.Select(r => r.ToViewModel()).ToListAsync();
            return Ok(roles);
        }

        public async Task<IActionResult> Create(RoleViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbRole = new ApplicationRole()
            {
                Name = role.Name
            };

            var identityResult = await _roleManager.CreateAsync(dbRole);
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }

            return Ok(dbRole.ToViewModel());
        }

        public async Task<IActionResult> Update(int roleId, RoleViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbRole = new ApplicationRole()
            {
                Id = roleId,
                Name = role.Name
            };

            var identityResult = await _roleManager.UpdateAsync(dbRole);
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }

            return Ok(dbRole.ToViewModel());
        }

        public async Task<IActionResult> Delete(int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            await _roleManager.DeleteAsync(role);

            return Ok();
        }
    }
}
