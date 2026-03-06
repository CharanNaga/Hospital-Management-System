using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Hospital.DischargeService.Services
{
    public class GeminiSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = "gemini-2.0-flash";
        public int MaxTokens { get; set; } = 400;
        public int TimeoutSecs { get; set; } = 30;
    }

    public class AIDietService : IAIDietService
    {
        private readonly HttpClient _http;
        private readonly GeminiSettings _settings;
        private readonly ILogger<AIDietService> _logger;

        public AIDietService(
            IHttpClientFactory httpFactory,
            IOptions<GeminiSettings> options,
            ILogger<AIDietService> logger)
        {
            _http = httpFactory.CreateClient("gemini");
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<string> GenerateDietAsync(string diagnosis, int patientAge)
        {
            var prompt = $@"You are a clinical dietitian writing a discharge diet recommendation.
Patient details:
- Diagnosis: {diagnosis}
- Age: {patientAge} years

Write ONE concise paragraph (3-5 sentences) with practical dietary advice for this patient.
Mention key foods to include, foods to avoid, and fluid intake if relevant.
Plain prose only — no bullet points, no headers, no disclaimers.";

            var requestBody = JsonSerializer.Serialize(new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[] { new { text = prompt } }
                    }
                },
                generationConfig = new
                {
                    maxOutputTokens = _settings.MaxTokens,
                    temperature = 0.4
                }
            });

            //var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";
            var url = $"https://generativelanguage.googleapis.com/v1/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_settings.TimeoutSecs));
                var response = await _http.SendAsync(request, cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync(cts.Token);
                    _logger.LogError("Gemini API error {Status}: {Body}", response.StatusCode, errorBody);
                    return FallbackDiet(diagnosis, patientAge);
                }

                var json = await response.Content.ReadAsStringAsync(cts.Token);
                using var doc = JsonDocument.Parse(json);

                // Gemini response path: candidates[0].content.parts[0].text
                var text = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString() ?? string.Empty;

                _logger.LogInformation("Gemini API diet generated for: {Diagnosis} (age {Age})", diagnosis, patientAge);
                return text.Trim();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gemini API call failed — using fallback diet");
                return FallbackDiet(diagnosis, patientAge);
            }
        }

        // Rule-based fallback (used when Gemini API is unavailable)
        private static string FallbackDiet(string diagnosis, int patientAge)
        {
            var d = diagnosis.ToLowerInvariant();

            if (d.Contains("diabetes") || d.Contains("diabetic"))
                return "Follow a sugar-controlled diet: choose low glycaemic-index foods, increase high-fibre vegetables, favour lean proteins, and avoid refined carbohydrates and sugary drinks. Aim for 3 regular meals per day.";
            if (d.Contains("hypertension") || d.Contains("cardiac") || d.Contains("heart"))
                return "Follow the DASH dietary plan: limit sodium to under 2g per day, include potassium-rich foods such as bananas and sweet potatoes, and avoid processed and fried foods. Drink at least 2 litres of water daily.";
            if (d.Contains("kidney") || d.Contains("renal"))
                return "Follow a renal diet: limit potassium, phosphorus and protein as directed. Avoid high-potassium fruits and limit dairy. Monitor fluid intake closely.";
            if (d.Contains("liver") || d.Contains("hepat"))
                return "Follow a liver-friendly diet: keep fat intake low, ensure adequate protein, completely avoid alcohol, and favour complex carbohydrates and fresh vegetables.";
            if (d.Contains("anaemia") || d.Contains("anemia"))
                return "Increase iron-rich foods including leafy greens, red meat (in moderation), lentils, and fortified cereals. Pair with vitamin C to enhance absorption.";
            if (patientAge < 18)
                return "Follow a paediatric recovery diet rich in protein from dairy, eggs, and lean meats. Avoid processed snacks and sugary drinks. Ensure adequate calcium and vitamin D.";
            if (patientAge >= 65)
                return "Follow a senior nutrition plan rich in calcium and vitamin D. Include adequate protein (1-1.2g per kg body weight) to prevent muscle loss. Stay hydrated with at least 1.5 litres of water daily.";

            return "Follow a balanced recovery diet providing adequate protein, colourful vegetables, whole grains, and seasonal fruits. Drink 2-2.5 litres of water per day and eat at regular intervals.";
        }
    }
}