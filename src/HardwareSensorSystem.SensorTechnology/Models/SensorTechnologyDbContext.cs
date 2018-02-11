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

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of measured values.
        /// </summary>
        public DbSet<MeasuredValue> MeasuredValues { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of measured values per day.
        /// </summary>
        public DbSet<MeasuredValuePerDay> MeasuredValuesPerDay { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of measured values per week.
        /// </summary>
        public DbSet<MeasuredValuePerWeek> MeasuredValuesPerWeek { get; set; }

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

                b.HasIndex(s => s.DeviceId).HasName("SensorDeviceIndex");
                b.HasOne<Device>().WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<SensorProperty>(b =>
            {
                b.ToTable("SensorProperties");

                b.HasKey(sp => sp.Id);
                b.Property(sp => sp.Name).HasMaxLength(256);
                b.Property(sp => sp.Value).HasMaxLength(256);

                b.HasIndex(s => s.SensorId).HasName("SensorPropertySensorIndex");
                b.HasOne<Sensor>().WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<MeasuredValue>(b =>
            {
                b.ToTable("MeasuredValues");

                b.HasKey(mv => new { mv.SensorId, mv.QuantityId, mv.Date, mv.Time });

                b.HasOne<Sensor>().WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<MeasuredValuePerDay>(b =>
            {
                b.ToTable("MeasuredValuesPerDay");

                b.HasKey(mvd => new { mvd.SensorId, mvd.QuantityId, mvd.Date });

                b.HasOne<Sensor>().WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<MeasuredValuePerWeek>(b =>
            {
                b.ToTable("MeasuredValuesPerWeek");

                b.HasKey(mvw => new { mvw.SensorId, mvw.QuantityId, mvw.Year, mvw.Week });

                b.HasOne<Sensor>().WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
