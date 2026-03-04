namespace Hospital.DoctorService.DTOs;

public record CreateDoctorDto(string FullName, string Specialization, string Email);
public record UpdateDoctorDto(string FullName, string Specialization, string Email);
