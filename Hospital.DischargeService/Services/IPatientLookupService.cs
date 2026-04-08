using Hospital.DischargeService.DTOs;

namespace Hospital.DischargeService.Services
{
    public interface IPatientLookupService
    {
        Task<PatientDetails?> GetPatientAsync(Guid patientId);
    }
}
