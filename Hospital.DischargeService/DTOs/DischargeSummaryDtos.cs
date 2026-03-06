namespace Hospital.DischargeService.DTOs;

public record DischargeSummaryDto(
    Guid PatientId,
    string PatientName,
    int PatientAge,
    string PatientGender,
    string Diagnosis,
    string Treatment,
    string Medications,
    string AIDietRecommendation,
    string FollowUpInstructions,
    Guid DischargingDoctorId
);
