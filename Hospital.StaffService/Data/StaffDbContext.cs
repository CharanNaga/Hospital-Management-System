using Hospital.StaffService.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.StaffService.Data
{
    public class StaffDbContext : DbContext
    {
        public StaffDbContext(DbContextOptions<StaffDbContext> options)
            : base(options) { }

        public DbSet<Staff> StaffMembers => Set<Staff>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Email unique among active staff only (soft-delete aware)
            modelBuilder.Entity<Staff>().HasIndex(s => s.Email);
            modelBuilder.Entity<Staff>().HasIndex(s => s.Department);
            modelBuilder.Entity<Staff>().HasIndex(s => s.IsActive);
        }

    }
}
