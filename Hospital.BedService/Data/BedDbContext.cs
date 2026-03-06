using Hospital.BedService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class BedDbContext : DbContext
{
    public BedDbContext(DbContextOptions<BedDbContext> options)
        : base(options) { }

    public DbSet<Bed> Beds => Set<Bed>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bed>()
            .HasIndex(b => b.BedNumber)
            .IsUnique();

        modelBuilder.Entity<Bed>()
            .HasIndex(b => b.Ward);
    }

}