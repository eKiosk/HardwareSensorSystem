using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HardwareSensorSystem.Security.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of permissions.
        /// </summary>
        public DbSet<ApplicationPermission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationPermission>(b =>
            {
                b.HasKey(p => p.Id);
                b.HasIndex(p => p.Name).HasName("PermissionNameIndex").IsUnique();
                b.ToTable("Permissions");

                b.Property(p => p.Name).HasMaxLength(256);
            });
        }
    }
}
