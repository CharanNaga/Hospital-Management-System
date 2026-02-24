namespace Hospital.BedService.Models
{
    public class Bed
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string BedNumber { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public bool IsOccupied { get; set; }
        public Guid? PatientId { get; set; }
        public DateTime? OccupiedSince { get; set; }
    }
}
