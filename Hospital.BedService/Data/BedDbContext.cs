using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class BedDbContext : DbContext
{
    public BedDbContext(DbContextOptions<BedDbContext> options)
        : base(options) { }

    public DbSet<Bed> Beds => Set<Bed>();
}