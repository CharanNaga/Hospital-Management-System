using Hospital.AppointmentService.DTOs;
using System.Text.Json;

namespace Hospital.AppointmentService.Services
{
    public class DoctorLookupService : IDoctorLookupService
    {
        private readonly HttpClient _http;
        private readonly ILogger<DoctorLookupService> _logger;
        private static readonly JsonSerializerOptions _json =
            new() { PropertyNameCaseInsensitive = true };

        public DoctorLookupService(IHttpClientFactory factory, ILogger<DoctorLookupService> logger)
        {
            _http = factory.CreateClient("DoctorService");
            _logger = logger;
        }

        public async Task<DoctorInfo?> GetDoctorAsync(Guid doctorId)
        {
            try
            {
                var response = await _http.GetAsync($"api/doctors/{doctorId}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("DoctorService returned {Status} for doctor {Id}",
                        response.StatusCode, doctorId);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var doctor = JsonSerializer.Deserialize<DoctorLookupResponse>(json, _json);
                return doctor is null ? null : new DoctorInfo(doctor.FullName, doctor.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reach DoctorService for doctor {Id}", doctorId);
                return null;
            }
        }
        private record DoctorLookupResponse(string FullName, string Email);

    }
}
