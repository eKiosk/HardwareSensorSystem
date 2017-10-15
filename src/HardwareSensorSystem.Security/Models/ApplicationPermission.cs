namespace HardwareSensorSystem.Security.Models
{
    /// <summary>
    /// Represents a permission.
    /// </summary>
    public class ApplicationPermission
    {
        /// <summary>
        /// Gets or sets the primary key for this permission.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name for this permission.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the name of the permission.
        /// </summary>
        /// <returns>The name of the permission.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
