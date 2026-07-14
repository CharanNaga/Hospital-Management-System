using Hospital.DischargeService.Models;

namespace Hospital.DischargeService.Repositories
{
    public interface IDischargeRepository
    {
        Task<IEnumerable<DischargeSummary>> GetAllAsync();
        Task<DischargeSummary?> GetByIdAsync(Guid id);
        Task<IEnumerable<DischargeSummary>> GetByPatientIdAsync(Guid patientId);
        Task<IEnumerable<DischargeSummary>> GetByDoctorIdAsync(Guid doctorId);
        Task AddAsync(DischargeSummary summary);
        Task SaveChangesAsync();
    }

}
