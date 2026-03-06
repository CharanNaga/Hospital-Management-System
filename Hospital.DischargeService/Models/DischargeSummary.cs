namespace Hospital.DischargeService.Models
{
    public class DischargeSummary
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int PatientAge { get; set; }

        public string PatientGender { get; set; } = string.Empty;   
        public DateTime? AdmittedOn { get; set; }

        public string Diagnosis { get; set; } = string.Empty;
        public string Treatment { get; set; } = string.Empty;
        public string Medications { get; set; } = string.Empty;
        public string AIDietRecommendation { get; set; } = string.Empty;
        public string FollowUpInstructions { get; set; } = string.Empty;
        public DateTime DischargedOn { get; set; } = DateTime.UtcNow;
        public Guid DischargingDoctorId { get; set; }
    }
}
