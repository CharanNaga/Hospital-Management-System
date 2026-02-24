using Hospital.DoctorService.Models;

public interface IDoctorRepository
{
    Task<IEnumerable<Doctor>> GetAllAsync();
    Task<Doctor?> GetByIdAsync(Guid id);
    Task AddAsync(Doctor doctor);
    void Remove(Doctor doctor);
    Task SaveChangesAsync();
}
