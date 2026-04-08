namespace Hospital.DischargeService.DTOs;

public record DischargeSummaryDto(
    Guid PatientId,
    //string PatientName,
    //int PatientAge,
    //string PatientGender,
    DateTime? AdmittedOn,
    string Diagnosis,
    string Treatment,
    string Medications,
    //string AIDietRecommendation,
    string FollowUpInstructions,
    Guid DischargingDoctorId
);

public record PatientDetails(
    string FullName,
    int Age,
    string Gender,
    string Email);
