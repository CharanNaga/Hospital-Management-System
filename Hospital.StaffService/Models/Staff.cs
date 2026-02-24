namespace Hospital.StaffService.Models
{
    public class Staff
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;      // Nurse | Technician | Admin | Porter
        public string Department { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Shift { get; set; } = "Day";            // Day | Night | Rotating
        public bool IsActive { get; set; } = true;
        public DateTime JoinedOn { get; set; } = DateTime.UtcNow;
    }
}
