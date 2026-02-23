using Microsoft.EntityFrameworkCore;

namespace Hospital.DischargeService.Data
{
    public class DischargeDbContext : DbContext
    {
        public DischargeDbContext(DbContextOptions<DischargeDbContext> options)
            : base(options) { }

        public DbSet<DischargeSummary> DischargeSummaries => Set<DischargeSummary>();
    }
}
