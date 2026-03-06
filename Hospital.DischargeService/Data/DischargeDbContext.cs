using Hospital.DischargeService.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.DischargeService.Data
{
    public class DischargeDbContext : DbContext
    {
        public DischargeDbContext(DbContextOptions<DischargeDbContext> options)
            : base(options) { }

        public DbSet<DischargeSummary> DischargeSummaries => Set<DischargeSummary>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DischargeSummary>()
                .HasIndex(d => d.PatientId);

            modelBuilder.Entity<DischargeSummary>()
                .HasIndex(d => d.DischargedOn);
        }

    }
}
