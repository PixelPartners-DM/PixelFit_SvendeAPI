using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

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

        public DiscordWebhookService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _webhookUrl = config["Discord:WebhookUrl"] ?? string.Empty;
        }

        public async Task SendLoginNotificationAsync(string email, string? userId, string ip, bool success, string? failureReason = null)
        {
            if (string.IsNullOrWhiteSpace(_webhookUrl))
                return;

            var contentText = success
                ? $"✅ Successful login: `{email}` (id: `{userId ?? "unknown"}`) from `{ip}`"
                : $"❌ Failed login: `{email}` from `{ip}`. Reason: {failureReason ?? "unknown"}";

            var payload = new { content = contentText };

            var json = JsonSerializer.Serialize(payload);
            using var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            await _http.PostAsync(_webhookUrl, httpContent);
        }
    }
}