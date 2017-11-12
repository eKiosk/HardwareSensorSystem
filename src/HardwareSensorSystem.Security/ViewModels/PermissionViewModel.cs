using HardwareSensorSystem.Security.Models;

namespace HardwareSensorSystem.Security.ViewModels
{
    /// <summary>
    /// View model of a permission.
    /// </summary>
    public class PermissionViewModel : ApplicationPermission
    { }

    public static class PermissionViewModelExtensions
    {
        /// <summary>
        /// Convert an <see cref="ApplicationPermission"/> to its view model.
        /// </summary>
        /// <param name="appPermission">The permission.</param>
        /// <returns>A view model of the permission.</returns>
        public static PermissionViewModel ToViewModel(this ApplicationPermission appPermission)
        {
            return new PermissionViewModel()
            {
                Id = appPermission.Id,
                Name = appPermission.Name
            };
        }
    }
}
