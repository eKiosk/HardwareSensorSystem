using Microsoft.EntityFrameworkCore;

namespace HardwareSensorSystem.SensorTechnology.Models
{
    public class SensorTechnologyDbContext : DbContext
    {
        public SensorTechnologyDbContext(DbContextOptions<SensorTechnologyDbContext> options)
            : base(options)
        { }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of devices.
        /// </summary>
        public DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Device>(b =>
            {
                b.HasKey(d => d.Id);
                b.HasIndex(d => d.NormalizedName).HasName("DeviceNameIndex").IsUnique();
                b.ToTable("Devices");

                b.Property(d => d.Name).HasMaxLength(256);
                b.Property(d => d.NormalizedName).HasMaxLength(256);
            });
        }
    }
}
