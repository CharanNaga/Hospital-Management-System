namespace Hospital.StaffService.DTOs;

public record CreateStaffDto(
    string FullName,
    string Role,
    string Department,
    string Email,
    string Phone,
    string Shift);

public record UpdateStaffDto(
    string FullName,
    string Role,
    string Department,
    string Email,
    string Phone,
    string Shift);
