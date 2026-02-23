using Hospital.PatientService.Models;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllAsync();
    Task<Patient?> GetByIdAsync(Guid id);
    Task AddAsync(Patient patient);
    Task SaveChangesAsync();
}