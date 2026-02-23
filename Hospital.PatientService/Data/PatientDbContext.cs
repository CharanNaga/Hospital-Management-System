using Hospital.PatientService.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.PatientService.Data;

public class PatientDbContext : DbContext
{
    public PatientDbContext(DbContextOptions<PatientDbContext> options)
        : base(options) { }

    public DbSet<Patient> Patients => Set<Patient>();
}