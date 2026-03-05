using Hospital.PatientService.Models;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllAsync();
    Task<Patient?> GetByIdAsync(Guid id);
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);

    Task AddAsync(Patient patient);
    void Remove(Patient patient);
    Task SaveChangesAsync();
}
