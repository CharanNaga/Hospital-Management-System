using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Hospital.DischargeService.Services
{// ─── Configuration ───────────────────────────────────────────────────────────
    public class GroqSettings
    {
        // Free tier: ~14,400 requests/day, no billing required
        // Get key at: https://console.groq.com → API Keys → Create API Key
        public string ApiKey { get; set; } = string.Empty;

        // llama-3.3-70b-versatile: best quality, free
        // fallback options: llama-3.1-8b-instant (faster), mixtral-8x7b-32768
        public string Model { get; set; } = "llama-3.3-70b-versatile";
        public int MaxTokens { get; set; } = 400;
        public int TimeoutSecs { get; set; } = 30;
    }

    // ─── Groq AI Diet Service (OpenAI-compatible API) ────────────────────────────
    // Groq provides free, fast LLaMA inference with no credit card required.
    // The API is OpenAI-compatible so the request/response format is familiar.
    public class AIDietService : IAIDietService
    {
        private readonly HttpClient _http;
        private readonly GroqSettings _settings;
        private readonly ILogger<AIDietService> _logger;

        public AIDietService(
            IHttpClientFactory httpFactory,
            IOptions<GroqSettings> options,
            ILogger<AIDietService> logger)
        {
            _http = httpFactory.CreateClient("groq");
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<string> GenerateDietAsync(string diagnosis, int patientAge)
        {
            var prompt = $@"You are a clinical dietitian writing a discharge diet recommendation for a hospital patient.

Patient details:
- Diagnosis: {diagnosis}
- Age: {patientAge} years

Write ONE concise paragraph (3-5 sentences) with specific, practical dietary advice tailored to this patient's diagnosis and age group. Mention key foods to include and foods to avoid. Include daily fluid intake recommendation if relevant. Do NOT include disclaimers, headers, or bullet points — plain prose only. Keep the language simple and accessible for a non-medical patient.";

            // Groq uses OpenAI-compatible /chat/completions format
            var requestBody = JsonSerializer.Serialize(new
            {
                model = _settings.Model,
                max_tokens = _settings.MaxTokens,
                temperature = 0.4,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            });

            using var request = new HttpRequestMessage(HttpMethod.Post,
                "https://api.groq.com/openai/v1/chat/completions")
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {_settings.ApiKey}");

            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_settings.TimeoutSecs));
                var response = await _http.SendAsync(request, cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync(cts.Token);
                    _logger.LogError("Groq API error {Status}: {Body}", response.StatusCode, errorBody);
                    return FallbackDiet(diagnosis, patientAge);
                }

                var json = await response.Content.ReadAsStringAsync(cts.Token);
                using var doc = JsonDocument.Parse(json);

                // OpenAI-compatible response: choices[0].message.content
                var text = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? string.Empty;

                _logger.LogInformation(
                    "Groq AI diet generated for diagnosis '{Diagnosis}' (age {Age})", diagnosis, patientAge);
                return text.Trim();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Groq API call failed — using rule-based fallback diet");
                return FallbackDiet(diagnosis, patientAge);
            }
        }

        // ── Rule-based fallback (used when Groq API is unavailable) ─────────────
        private static string FallbackDiet(string diagnosis, int patientAge)
        {
            var d = diagnosis.ToLowerInvariant();

            if (d.Contains("diabetes") || d.Contains("diabetic"))
                return "Follow a sugar-controlled diet: choose low glycaemic-index foods, increase high-fibre vegetables such as broccoli and spinach, favour lean proteins, and avoid refined carbohydrates and sugary drinks. Aim for 3 regular meals per day and limit snacks.";
            if (d.Contains("hypertension") || d.Contains("cardiac") || d.Contains("heart"))
                return "Follow the DASH dietary plan: limit sodium intake to under 2g per day, include potassium-rich foods such as bananas and sweet potatoes, and avoid processed foods, fried items, and saturated fats. Drink at least 2 litres of water daily.";
            if (d.Contains("kidney") || d.Contains("renal"))
                return "Follow a renal diet: limit potassium, phosphorus, and protein intake as directed by your nephrologist. Avoid high-potassium fruits such as oranges and bananas, and limit dairy products. Monitor fluid intake closely.";
            if (d.Contains("liver") || d.Contains("hepat"))
                return "Follow a liver-friendly diet: keep fat intake low, ensure adequate protein (0.8-1g per kg body weight), completely avoid alcohol, and favour complex carbohydrates and fresh vegetables. Eat small, frequent meals.";
            if (d.Contains("anaemia") || d.Contains("anemia"))
                return "Increase iron-rich foods including leafy greens, red meat (in moderation), lentils, and fortified cereals. Pair iron-rich foods with a source of vitamin C to enhance absorption. Avoid tea and coffee with meals as they inhibit iron uptake.";
            if (patientAge < 18)
                return "Follow a paediatric recovery diet rich in protein from dairy, eggs, and lean meats to support growth and healing. Avoid processed snacks and sugary drinks. Ensure adequate calcium and vitamin D intake.";
            if (patientAge >= 65)
                return "Follow a senior nutrition plan rich in calcium and vitamin D to support bone health. Include adequate protein (1-1.2g per kg body weight) to prevent muscle loss. Stay well-hydrated with at least 1.5 litres of water daily.";

            return "Follow a balanced recovery diet providing adequate protein (1g per kg body weight), colourful vegetables, whole grains, and seasonal fruits. Drink 2-2.5 litres of water per day, avoid alcohol and smoking, and eat at regular intervals.";
        }
    }
}