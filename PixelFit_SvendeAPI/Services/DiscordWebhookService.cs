using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PixelFit_SvendeAPI.Services
{
    public interface IDiscordWebhookService
    {
        Task SendLoginNotificationAsync(string email, string? userId, string ip, bool success, string? failureReason = null);
    }

    public class DiscordWebhookService : IDiscordWebhookService
    {
        private readonly HttpClient _http;
        private readonly string _webhookUrl;
        private readonly ILogger<DiscordWebhookService> _logger;

        public DiscordWebhookService(HttpClient http, IConfiguration config, ILogger<DiscordWebhookService> logger)
        {
            _http = http;
            _webhookUrl = config["Discord:WebhookUrl"] ?? string.Empty;
            _logger = logger;
        }

        public async Task SendLoginNotificationAsync(string email, string? userId, string ip, bool success, string? failureReason = null)
        {
            if (string.IsNullOrWhiteSpace(_webhookUrl))
                return;

            var contentText = success
                ? $"✅ Successful login: `{email}` (id: `{userId ?? "unknown"}`) from `{ip}`"
                : $"❌ Failed login: `{email}` from `{ip}`. Reason: {failureReason ?? "unknown"}";

            var payload = new { content = contentText };

            try
            {
                var json = JsonSerializer.Serialize(payload);
                using var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await _http.PostAsync(_webhookUrl, httpContent);
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Discord webhook responded {StatusCode} for payload: {Payload}", resp.StatusCode, contentText);
                }
            }
            catch (Exception ex)
            {
                // Swallow exceptions so webhook failures do not affect callers
                _logger.LogWarning(ex, "Failed to send Discord webhook for login notification for {Email}", email);
            }
        }
    }
}