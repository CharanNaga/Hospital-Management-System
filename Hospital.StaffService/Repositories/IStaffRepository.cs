using Hospital.StaffService.Models;

namespace Hospital.StaffService.Repositories
{
    public interface IStaffRepository
    {
        Task<IEnumerable<Staff>> GetAllAsync();
        Task<Staff?> GetByIdAsync(Guid id);
        Task<IEnumerable<Staff>> GetByDepartmentAsync(string department);
        Task AddAsync(Staff staff);
        void Remove(Staff staff);
        Task SaveChangesAsync();
    }

}
