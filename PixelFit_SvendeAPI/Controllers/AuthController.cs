using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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


        // Dependency Injection giver controlleren adgang til
        // brugerhåndtering, JWT-service og logging
        public AuthController(
            UserManager<User> userManager,
            JwtService jwtService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _logger = logger;
        }



        [HttpPost("login")]
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