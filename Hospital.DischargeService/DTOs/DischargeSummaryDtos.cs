namespace Hospital.DischargeService.DTOs;

public record DischargeSummaryDto(
    Guid PatientId,
    string PatientName,
    int PatientAge,
    string Diagnosis,
    string Treatment,
    string Medications,
    string FollowUpInstructions,
    Guid DischargingDoctorId
);
