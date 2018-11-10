using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MCSeatScheduler
{
    public partial class MCDBContext : DbContext
    {
        public MCDBContext()
        {
        }

        public MCDBContext(DbContextOptions<MCDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClearingOpenSeats> ClearingOpenSeats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseNpgsql("Host=192.168.0.156;Database=mastercard_etc;Username=mps;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClearingOpenSeats>(entity =>
            {
                entity.HasKey(e => new { e.Date, e.EmployeeId });

                entity.ToTable("clearing_open_seats");

                entity.ForNpgsqlHasComment("table that holds information regarding seats available in room during clearing renovation");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .HasMaxLength(8);
            });
        }
    }
}
