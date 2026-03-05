using Hospital.BedService.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.BedService.Repositories
{
    public class BedRepository : IBedRepository
    {
        private readonly BedDbContext _context;

        public BedRepository(BedDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bed>> GetAllAsync()
            => await _context.Beds.ToListAsync();

        public async Task<IEnumerable<Bed>> GetAvailableAsync()
            => await _context.Beds.Where(b => !b.IsOccupied).ToListAsync();

        public async Task<Bed?> GetByIdAsync(Guid id)
            => await _context.Beds.FindAsync(id);

        public async Task<bool> BedNumberExistsAsync(string bedNumber, Guid? excludeId = null)
        => await _context.Beds.AnyAsync(b =>
            b.BedNumber.ToUpper() == bedNumber.ToUpper() && b.Id != excludeId);


        public async Task AddAsync(Bed bed)
            => await _context.Beds.AddAsync(bed);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }

}
