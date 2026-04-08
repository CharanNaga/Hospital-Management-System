namespace Hospital.AppointmentService.DTOs;

public record CreateAppointmentDto(
    Guid PatientId,
    //string PatientName,
    //string PatientEmail,

    Guid DoctorId,

    //string DoctorName,
    //string DoctorEmail,
    DateTime AppointmentDate,
    string? Notes
);

public record UpdateAppointmentStatusDto(
    string Status
);

public record PatientInfo(string FullName, string Email);
public record DoctorInfo(string FullName, string Email);
