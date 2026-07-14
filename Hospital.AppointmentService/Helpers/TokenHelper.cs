using System.Net.Http.Headers;

namespace Hospital.AppointmentService.Helpers
{
    // ─── Helper: extract JWT from the current HTTP request ───────────────────────
    // PatientService and DoctorService both require [Authorize].
    // When AppointmentService calls them internally, it must forward the same
    // Bearer token that the original caller sent — otherwise the request arrives
    // unauthenticated and both services return 401 Unauthorized.

    public static class TokenHelper
    {
        /// <summary>
        /// Reads the raw "Bearer eyJ..." value from the current request's
        /// Authorization header and attaches it to the outgoing HttpRequestMessage.
        /// If no token is present the request is sent without a header and will
        /// receive a 401 — which is caught and logged as a warning.
        /// </summary>
        public static void ForwardToken(
            HttpRequestMessage outgoing,
            IHttpContextAccessor accessor,
            ILogger logger)
        {
            var authHeader = accessor.HttpContext?
                .Request.Headers["Authorization"]
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(authHeader))
            {
                logger.LogWarning(
                    "No Authorization header found on the incoming request — " +
                    "outgoing service-to-service call will be unauthenticated.");
                return;
            }

            // authHeader is the full string: "Bearer eyJhbGc..."
            // HttpClient.DefaultRequestHeaders.Authorization wants just the token part,
            // but the cleanest approach is to set the raw header value directly.
            outgoing.Headers.Authorization =
                AuthenticationHeaderValue.Parse(authHeader);
        }
    }
}
