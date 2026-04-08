using Hospital.AppointmentService.DTOs;
using System.Text.Json;

namespace Hospital.AppointmentService.Services
{
    public class PatientLookupService : IPatientLookupService
    {
        private readonly HttpClient _http;
        private readonly ILogger<PatientLookupService> _logger;
        private static readonly JsonSerializerOptions _json =
            new() { PropertyNameCaseInsensitive = true };

        public PatientLookupService(IHttpClientFactory factory, ILogger<PatientLookupService> logger)
        {
            _http = factory.CreateClient("PatientService");
            _logger = logger;
        }

        public async Task<PatientInfo?> GetPatientAsync(Guid patientId)
        {
            try
            {
                var response = await _http.GetAsync($"api/patients/{patientId}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PatientService returned {Status} for patient {Id}",
                        response.StatusCode, patientId);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var patient = JsonSerializer.Deserialize<PatientLookupResponse>(json, _json);
                return patient is null ? null : new PatientInfo(patient.FullName, patient.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reach PatientService for patient {Id}", patientId);
                return null;
            }
        }

        // Internal deserialization model matching PatientService response shape
        private record PatientLookupResponse(string FullName, string Email);

    }
}
