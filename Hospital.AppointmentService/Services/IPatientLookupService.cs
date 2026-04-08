using Hospital.AppointmentService.DTOs;

namespace Hospital.AppointmentService.Services
{
    public interface IPatientLookupService
    {
        Task<PatientInfo?> GetPatientAsync(Guid patientId);

    }
}
