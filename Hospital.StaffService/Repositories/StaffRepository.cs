using Hospital.StaffService.Data;
using Hospital.StaffService.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.StaffService.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly StaffDbContext _context;

        public StaffRepository(StaffDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Staff>> GetAllAsync()
            => await _context.StaffMembers.Where(s => s.IsActive).ToListAsync();

        public async Task<Staff?> GetByIdAsync(Guid id)
            => await _context.StaffMembers.FindAsync(id);

        public async Task<IEnumerable<Staff>> GetByDepartmentAsync(string department)
            => await _context.StaffMembers
                .Where(s => s.Department == department && s.IsActive)
                .ToListAsync();

        public async Task AddAsync(Staff staff)
            => await _context.StaffMembers.AddAsync(staff);

        public void Remove(Staff staff)
            => _context.StaffMembers.Remove(staff);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }

}
