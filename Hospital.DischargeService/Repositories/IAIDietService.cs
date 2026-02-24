namespace Hospital.DischargeService.Repositories
{
    public interface IAIDietService
    {
        Task<string> GenerateDietAsync(string diagnosis, int patientAge);
    }

}
