namespace Hospital.DischargeService.Repositories
{
    public class AIDietService : IAIDietService
    {
        public string GenerateDiet(string diagnosis)
        {
            if (diagnosis.Contains("Diabetes"))
                return "Low sugar diet, high fiber vegetables.";
            if (diagnosis.Contains("Hypertension"))
                return "Low sodium diet.";
            return "Balanced protein rich diet.";
        }
    }
}
