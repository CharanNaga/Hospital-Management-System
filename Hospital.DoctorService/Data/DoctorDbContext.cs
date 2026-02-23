using Hospital.DoctorService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class DoctorDbContext : DbContext
{
    public DoctorDbContext(DbContextOptions<DoctorDbContext> options)
        : base(options) { }

    public DbSet<Doctor> Doctors => Set<Doctor>();
}