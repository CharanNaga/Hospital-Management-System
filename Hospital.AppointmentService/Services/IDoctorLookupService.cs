using Hospital.AppointmentService.DTOs;

namespace Hospital.AppointmentService.Services
{
    public interface IDoctorLookupService
    {
        Task<DoctorInfo?> GetDoctorAsync(Guid doctorId);
    }
}
