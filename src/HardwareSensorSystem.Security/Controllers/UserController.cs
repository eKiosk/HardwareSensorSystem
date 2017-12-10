using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareSensorSystem.Security.Controllers
{
    public class UserController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.Select(u => u.ToViewModel()).ToListAsync();
            return Ok(users);
        }

        public async Task<IActionResult> Create(UserCreateViewModel user)
        {
            var dbUser = new ApplicationUser()
            {
                UserName = user.UserName,
                Email = user.Email
            };

            await _userManager.CreateAsync(dbUser, user.Password);

            var dbRole = await _roleManager.FindByIdAsync(user.RoleId.ToString());
            await _userManager.AddToRoleAsync(dbUser, dbRole.Name);

            return Ok(dbUser.ToViewModel());
        }
    }
}
