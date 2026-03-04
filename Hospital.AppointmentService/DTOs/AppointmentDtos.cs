namespace Hospital.AppointmentService.DTOs;

public record CreateAppointmentDto(
    Guid PatientId,
    Guid DoctorId,
    DateTime AppointmentDate,
    string? Notes
);

public record UpdateAppointmentStatusDto(
    string Status
);