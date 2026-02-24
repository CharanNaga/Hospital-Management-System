using Hospital.StaffService.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.StaffService.Data
{
    public class StaffDbContext : DbContext
    {
        public StaffDbContext(DbContextOptions<StaffDbContext> options)
            : base(options) { }

        public DbSet<Staff> StaffMembers => Set<Staff>();
    }
}
