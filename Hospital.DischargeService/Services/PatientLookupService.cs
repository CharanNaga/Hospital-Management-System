using Hospital.DischargeService.DTOs;
using System.Text.Json;

namespace Hospital.DischargeService.Services
{
    public class PatientLookupService : IPatientLookupService
    {
        private readonly HttpClient _http;
        private readonly ILogger<PatientLookupService> _logger;

        private static readonly JsonSerializerOptions _json =
            new() { PropertyNameCaseInsensitive = true };

        public PatientLookupService(
            IHttpClientFactory factory,
            ILogger<PatientLookupService> logger)
        {
            _http = factory.CreateClient("PatientService");
            _logger = logger;
        }

        public async Task<PatientDetails?> GetPatientAsync(Guid patientId)
        {
            try
            {
                var response = await _http.GetAsync($"api/patients/{patientId}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "PatientService returned {Status} for patient {Id}",
                        response.StatusCode, patientId);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var patient = JsonSerializer.Deserialize<PatientLookupResponse>(json, _json);

                return patient is null ? null
                    : new PatientDetails(patient.FullName, patient.Age, patient.Gender, patient.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to contact PatientService for patient {Id}", patientId);
                return null;
            }
        }

        private record PatientLookupResponse(
            string FullName,
            int Age,
            string Gender,
            string Email);

    }
}
