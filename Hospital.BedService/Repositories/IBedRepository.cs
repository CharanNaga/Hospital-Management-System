using Hospital.BedService.Models;
namespace Hospital.BedService.Repositories
{
    public interface IBedRepository
    {
        Task<IEnumerable<Bed>> GetAllAsync();
        Task<IEnumerable<Bed>> GetAvailableAsync();
        Task<Bed?> GetByIdAsync(Guid id);
        Task<bool> BedNumberExistsAsync(string bedNumber, Guid? excludeId = null);

        Task AddAsync(Bed bed);
        Task SaveChangesAsync();
    }
}
