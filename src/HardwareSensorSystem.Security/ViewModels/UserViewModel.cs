using HardwareSensorSystem.Security.Models;

namespace HardwareSensorSystem.Security.ViewModels
{
    /// <summary>
    /// View model of user.
    /// </summary>
    public class UserViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
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
                Email = appUser.Email
            };
        }
    }
}
