using Hospital.PatientService.Data;
using Hospital.PatientService.Models;
using Microsoft.EntityFrameworkCore;

public class PatientRepository : IPatientRepository
{
    private readonly PatientDbContext _context;

    public PatientRepository(PatientDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Patient>> GetAllAsync() =>
        await _context.Patients.ToListAsync();

    public async Task<Patient?> GetByIdAsync(Guid id) =>
        await _context.Patients.FindAsync(id);

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
        => await _context.Patients.AnyAsync(p =>
            p.Email.ToLower() == email.ToLower() && p.Id != excludeId);


    public async Task AddAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
    }

    public void Remove(Patient patient)
        => _context.Patients.Remove(patient);

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
