using Hospital.DoctorService.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.DoctorService.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DoctorDbContext _context;

        public DoctorRepository(DoctorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
            => await _context.Doctors.ToListAsync();

        public async Task AddAsync(Doctor doctor)
            => await _context.Doctors.AddAsync(doctor);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
