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

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of sensors.
        /// </summary>
        public DbSet<Sensor> Sensors { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of sensor properties.
        /// </summary>
        public DbSet<SensorProperty> SensorProperties { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Device>(b =>
            {
                b.ToTable("Devices");

                b.HasKey(d => d.Id);
                b.Property(d => d.Name).HasMaxLength(256);
                b.Property(d => d.NormalizedName).HasMaxLength(256);

                b.HasIndex(d => d.NormalizedName).IsUnique().HasName("DeviceNameIndex");
            });

            builder.Entity<Sensor>(b =>
            {
                b.ToTable("Sensors");

                b.HasKey(s => s.Id);
                b.Property(s => s.Name).HasMaxLength(256);

                b.HasIndex(s => s.DeviceId).ForSqlServerIsClustered().HasName("SensorDeviceIndex");
                b.HasOne<Device>().WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<SensorProperty>(b =>
            {
                b.ToTable("SensorProperties");

                b.HasKey(sp => sp.Id);
                b.Property(sp => sp.Name).HasMaxLength(256);
                b.Property(sp => sp.Value).HasMaxLength(256);

                b.HasIndex(s => s.SensorId).ForSqlServerIsClustered().HasName("SensorPropertySensorIndex");
                b.HasOne<Sensor>().WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
