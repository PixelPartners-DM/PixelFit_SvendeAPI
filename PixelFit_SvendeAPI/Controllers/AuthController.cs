using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Services;

namespace PixelFit_SvendeAPI.Controllers
{
    // Fortæller ASP.NET Core at dette er en API-controller
    [ApiController]

    // Endpoint bliver: api/auth
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        // Dependency Injection giver controlleren adgang til
        // brugerhåndtering og JWT-service
        public AuthController(
            UserManager<User> userManager,
            JwtService jwtService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            _logger.LogDebug("Login attempt for {Email} from {IP}", dto?.Email ?? "null", ip);

            // Finder brugeren ud fra email
            var user = await _userManager.FindByEmailAsync(dto.Email);

            // Hvis brugeren ikke findes
            if (user == null)
            {
                _logger.LogWarning("Failed login: unknown email {Email} from {IP}", dto.Email, ip);
                return Unauthorized(new
                {
                    message = "Forkert email eller adgangskode."
                });
            }

            // Tjekker om adgangskoden passer til den gemte hash
            var passwordCorrect =
                await _userManager.CheckPasswordAsync(
                    user,
                    dto.Password
                );

            // Hvis adgangskoden er forkert
            if (!passwordCorrect)
            {
                _logger.LogWarning("Failed login: invalid password for user id {UserId} ({Email}) from {IP}", user.Id, dto.Email, ip);
                return Unauthorized(new
                {
                    message = "Forkert email eller adgangskode."
                });
            }

            // Opretter JWT-token til brugeren
            var token = _jwtService.CreateToken(user);

            _logger.LogInformation("Successful login for user id {UserId} ({Email}) from {IP}", user.Id, user.Email, ip);

            // Sender token tilbage til MAUI-appen
            return Ok(new
            {
                token = token
            });
        }
    }
}