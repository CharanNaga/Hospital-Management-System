public class DischargeSummary
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string AIDietRecommendation { get; set; } = string.Empty;
    public DateTime DischargedOn { get; set; } = DateTime.UtcNow;
}