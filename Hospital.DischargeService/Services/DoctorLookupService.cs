using Hospital.DischargeService.DTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Hospital.DischargeService.Services
{
    // ─── Implementation ───────────────────────────────────────────────────────────
    // DoctorService requires [Authorize] on all endpoints.
    // This service reads the JWT from the current incoming HTTP request
    // (via IHttpContextAccessor) and forwards it in the outgoing call to
    // DoctorService — exactly like a browser forwards a cookie.
    public class DoctorLookupService : IDoctorLookupService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<DoctorLookupService> _logger;

        private static readonly JsonSerializerOptions _json =
            new() { PropertyNameCaseInsensitive = true };

        public DoctorLookupService(
            IHttpClientFactory factory,
            IHttpContextAccessor accessor,
            ILogger<DoctorLookupService> logger)
        {
            _http = factory.CreateClient("DoctorService");
            _accessor = accessor;
            _logger = logger;
        }

        public async Task<DoctorDetails?> GetDoctorAsync(Guid doctorId)
        {
            try
            {
                using var request = new HttpRequestMessage(
                HttpMethod.Get, $"api/Doctors/{doctorId}");

                // Forward the caller's JWT so PatientService accepts the request
                AttachToken(request);

                var response = await _http.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "DoctorService returned {Status} for doctor {Id}",
                        response.StatusCode, doctorId);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var doctor = JsonSerializer.Deserialize<DoctorLookupResponse>(json, _json);

                return doctor is null ? null
                    : new DoctorDetails(doctor.FullName, doctor.Specialization, doctor.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to contact DoctorService for doctor {Id}", doctorId);
                return null;
            }
        }

        // ── Reads the incoming Authorization header and copies it to the outgoing request
        private void AttachToken(HttpRequestMessage outgoing)
        {
            var authHeader = _accessor.HttpContext?
                .Request.Headers["Authorization"]
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(authHeader))
            {
                _logger.LogWarning(
                    "No Authorization header on incoming request — " +
                    "PatientService call will be unauthenticated and may return 401.");
                return;
            }

            outgoing.Headers.Authorization =
                AuthenticationHeaderValue.Parse(authHeader);
        }

        private record DoctorLookupResponse(
            string FullName,
            string Specialization,
            string Email);

    }
}
