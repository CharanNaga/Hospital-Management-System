using Hospital.DoctorService.Models;

namespace Hospital.DoctorService.Repositories
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task AddAsync(Doctor doctor);
        Task SaveChangesAsync();
    }
}
