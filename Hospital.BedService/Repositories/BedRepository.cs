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

        public async Task AddAsync(Bed bed)
            => await _context.Beds.AddAsync(bed);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
