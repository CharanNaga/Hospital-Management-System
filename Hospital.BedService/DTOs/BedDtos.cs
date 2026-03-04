namespace Hospital.BedService.DTOs;

public record CreateBedDto(string BedNumber, string Ward);
public record AssignBedDto(Guid PatientId);
