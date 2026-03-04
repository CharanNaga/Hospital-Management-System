namespace Hospital.PatientService.DTOs;

public record CreatePatientDto(
    string FullName, int Age, string Gender,
    string Phone, string Email, string Address);

public record UpdatePatientDto(
    string FullName, int Age, string Gender,
    string Phone, string Email, string Address);
