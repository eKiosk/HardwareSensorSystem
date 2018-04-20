using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareSensorSystem.Security.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("~/api/roles/{roleId}/users")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllInRole([FromRoute]int roleId)
        {
            var dbRole = await _roleManager.FindByIdAsync(roleId.ToString());
            var users = await _userManager.GetUsersInRoleAsync(dbRole.Name);
            return Ok(users.Select(user => user.ToViewModel()));
        }

        [HttpGet("{userId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetById([FromRoute]int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return Ok(user.ToViewModel());
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody]UserCreateViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbRole = await _roleManager.FindByIdAsync(user.RoleId.ToString());
            if (dbRole is null)
            {
                ModelState.AddModelError(nameof(user.RoleId), "Invalid role id.");
                return BadRequest(ModelState);
            }

            var dbUser = new ApplicationUser()
            {
                UserName = user.UserName,
                Email = user.Email
            };

            var identityResult = await _userManager.CreateAsync(dbUser, user.Password);
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }

            await _userManager.AddToRoleAsync(dbUser, dbRole.Name);

            return Ok(dbUser.ToViewModel());
        }

        [HttpPut("{userId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Update([FromRoute]int userId, [FromBody]UserUpdateViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationRole dbRole = null;
            if (user.RoleId.HasValue)
            {
                dbRole = await _roleManager.FindByIdAsync(user.RoleId.ToString());
                if (dbRole is null)
                {
                    ModelState.AddModelError(nameof(user.RoleId), "Invalid role id.");
                    return BadRequest(ModelState);
                }
            }

            var dbUser = await _userManager.FindByIdAsync(userId.ToString());
            dbUser.UserName = user.UserName;
            dbUser.Email = user.Email;
            dbUser.ConcurrencyStamp = user.ConcurrencyStamp;

            var identityResult = await _userManager.UpdateAsync(dbUser);
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                await _userManager.RemovePasswordAsync(dbUser);
                await _userManager.AddPasswordAsync(dbUser, user.Password);
            }

            if (dbRole != null)
            {
                var roleNames = await _userManager.GetRolesAsync(dbUser);
                await _userManager.RemoveFromRolesAsync(dbUser, roleNames);
                await _userManager.AddToRoleAsync(dbUser, dbRole.Name);
            }

            return Ok(dbUser.ToViewModel());
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete([FromRoute]int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            await _userManager.DeleteAsync(user);

            return Ok();
        }
    }
}
