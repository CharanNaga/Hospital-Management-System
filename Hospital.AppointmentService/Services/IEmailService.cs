namespace Hospital.AppointmentService.Services
{
    public interface IEmailService
    {
        Task SendAppointmentConfirmationAsync(
        string patientEmail, string patientName,
        string doctorName, string doctorEmail,
        DateTime appointmentDate, string notes);
    }
}
