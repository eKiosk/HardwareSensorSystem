using HardwareSensorSystem.Security.Models;
using System.ComponentModel.DataAnnotations;

namespace HardwareSensorSystem.Security.ViewModels
{
    /// <summary>
    /// View model of user.
    /// </summary>
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        public string ConcurrencyStamp { get; set; }
    }

    public class UserCreateViewModel : UserViewModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }
    }

    public class UserUpdateViewModel : UserViewModel
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public int RoleId { get; set; }
    }

    public static class UserViewModelExtensions
    {
        /// <summary>
        /// Convert an <see cref="ApplicationUser"/> to its view model.
        /// </summary>
        /// <param name="appRole">The user.</param>
        /// <returns>A view model of the user.</returns>
        public static UserViewModel ToViewModel(this ApplicationUser appUser)
        {
            return new UserViewModel()
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                ConcurrencyStamp = appUser.ConcurrencyStamp
            };
        }
    }
}
