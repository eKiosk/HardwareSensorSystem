using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HardwareSensorSystem.Security.Controllers
{
    [Route("api/roles")]
    public class RoleController : Controller
    {
        private RoleManager<ApplicationRole> _roleManager;

        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleManager.Roles.Select(r => r.ToViewModel()).ToListAsync();
            return Ok(roles);
        }

        [HttpGet("{roleId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetById([FromRoute]int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            return Ok(role.ToViewModel());
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody]RoleViewModel role)
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

        [HttpPut("{roleId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Update([FromRoute]int roleId, [FromBody]RoleViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbRole = new ApplicationRole()
            {
                Id = roleId,
                Name = role.Name,
                ConcurrencyStamp = role.ConcurrencyStamp
            };

            var identityResult = await _roleManager.UpdateAsync(dbRole);
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }

            return Ok(dbRole.ToViewModel());
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> Delete([FromRoute]int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            await _roleManager.DeleteAsync(role);

            return Ok();
        }

        [HttpPost("{roleId}/permissions/{permissionId}")]
        public async Task<IActionResult> AddPermission([FromRoute]int roleId, [FromRoute]int permissionId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            await _roleManager.AddClaimAsync(role, new Claim("Permission", permissionId.ToString()));

            return Ok();
        }

        [HttpDelete("{roleId}/permissions/{permissionId}")]
        public async Task<IActionResult> RemovePermission([FromRoute]int roleId, [FromRoute]int permissionId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            await _roleManager.RemoveClaimAsync(role, new Claim("Permission", permissionId.ToString()));

            return Ok();
        }
    }
}
