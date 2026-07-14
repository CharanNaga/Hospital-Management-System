using Hospital.DischargeService.DTOs;

namespace Hospital.DischargeService.Services
{
    public interface IDoctorLookupService
    {
        Task<DoctorDetails?> GetDoctorAsync(Guid doctorId);
    }
}
