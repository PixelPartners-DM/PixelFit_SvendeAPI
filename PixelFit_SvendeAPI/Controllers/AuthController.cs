using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Services;

namespace PixelFit_SvendeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;
        private readonly ILogger<AuthController> _logger;
        private readonly IDiscordWebhookService _discordWebhookService;


        // Dependency Injection giver controlleren adgang til
        // brugerhåndtering, JWT-service, logging og webhook-service
        public AuthController(
            UserManager<User> userManager,
            JwtService jwtService,
            ILogger<AuthController> logger,
            IDiscordWebhookService discordWebhookService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _logger = logger;
            _discordWebhookService = discordWebhookService;
        }



        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto dto)
        {
            // Henter IP-adressen på klienten
            var ip =
                HttpContext.Connection
                    .RemoteIpAddress?
                    .ToString()
                ??
                "unknown";


            // Logger at nogen forsøger at logge ind
            _logger.LogInformation(
                "Login attempt for {Email} from {IP}",
                dto.Email,
                ip
            );


            var user =
                await _userManager.FindByEmailAsync(
                    dto.Email
                );


            // Hvis brugeren ikke findes
            if (user == null)
            {
                _logger.LogWarning(
                    "Failed login: unknown email {Email} from {IP}",
                    dto.Email,
                    ip
                );


                // Notify discord (fire-and-forget so login latency isn't affected)
                _ = _discordWebhookService.SendLoginNotificationAsync(
                    dto.Email,
                    null,
                    ip,
                    success: false,
                    failureReason: "unknown email"
                );

                return Unauthorized(new
                {
                    message =
                        "Forkert email eller adgangskode."
                });
            }



            var passwordCorrect =
                await _userManager.CheckPasswordAsync(
                    user,
                    dto.Password
                );


            // Hvis adgangskoden er forkert
            if (!passwordCorrect)
            {
                _logger.LogWarning(
                    "Failed login: invalid password for user {UserId} from {IP}",
                    user.Id,
                    ip
                );


                // Notify discord (fire-and-forget)
                _ = _discordWebhookService.SendLoginNotificationAsync(
                    dto.Email,
                    user.Id.ToString(),
                    ip,
                    success: false,
                    failureReason: "invalid password"
                );

                return Unauthorized(new
                {
                    message =
                        "Forkert email eller adgangskode."
                });
            }

            _logger.LogInformation(
                "Successful login for user {UserId} from {IP}",
                user.Id,
                ip
            );

            // Notify discord (fire-and-forget)
            _ = _discordWebhookService.SendLoginNotificationAsync(
                user.Email,
                user.Id.ToString(),
                ip,
                success: true
            );


            // Opretter JWT-token til brugeren
            var token =
                _jwtService.CreateToken(
                    user
                );

            // Sender token tilbage til MAUI-appen
            return Ok(new
            {
                token = token
            });
        }
    }
}