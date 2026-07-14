using Hospital.AppointmentService.DTOs;
using Hospital.AppointmentService.Helpers;
using System.Text.Json;

namespace Hospital.AppointmentService.Services
{
    public class PatientLookupService : IPatientLookupService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<PatientLookupService> _logger;
        private static readonly JsonSerializerOptions _json =
            new() { PropertyNameCaseInsensitive = true };

        public PatientLookupService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<PatientLookupService> logger)
        {
            _http = factory.CreateClient("PatientService");
            _accessor = accessor;
            _logger = logger;
        }

        public async Task<PatientInfo?> GetPatientAsync(Guid patientId)
        {
            try
            {
                // Build request manually so we can attach the forwarded JWT token
                using var request = new HttpRequestMessage(
                    HttpMethod.Get, $"api/Patients/{patientId}");

                TokenHelper.ForwardToken(request, _accessor, _logger);

                var response = await _http.SendAsync(request);
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
