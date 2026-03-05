namespace Hospital.DischargeService.Services
{
    public interface IAIDietService
    {
        Task<string> GenerateDietAsync(string diagnosis, int patientAge);
    }

}
