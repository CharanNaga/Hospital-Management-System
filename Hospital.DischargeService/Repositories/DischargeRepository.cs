using Hospital.DischargeService.Data;
using Hospital.DischargeService.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.DischargeService.Repositories
{
    public class DischargeRepository : IDischargeRepository
    {
        private readonly DischargeDbContext _context;

        public DischargeRepository(DischargeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DischargeSummary>> GetAllAsync()
            => await _context.DischargeSummaries.ToListAsync();

        public async Task<DischargeSummary?> GetByIdAsync(Guid id)
            => await _context.DischargeSummaries.FindAsync(id);

        public async Task<IEnumerable<DischargeSummary>> GetByPatientIdAsync(Guid patientId)
            => await _context.DischargeSummaries
                .Where(d => d.PatientId == patientId)
                .OrderByDescending(d => d.DischargedOn)
                .ToListAsync();

        public async Task AddAsync(DischargeSummary summary)
            => await _context.DischargeSummaries.AddAsync(summary);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }

}
