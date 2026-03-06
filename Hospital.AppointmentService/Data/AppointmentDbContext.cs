using Hospital.AppointmentService.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.AppointmentService.Data
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options)
            : base(options) { }

        public DbSet<Appointment> Appointments => Set<Appointment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite index on DoctorId + AppointmentDate for fast conflict queries
            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.DoctorId, a.AppointmentDate });

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => a.PatientId);
        }

    }
}