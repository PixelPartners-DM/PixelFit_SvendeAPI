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

        // Dependency Injection giver controlleren adgang til
        // brugerhåndtering og JWT-service
        public AuthController(
            UserManager<User> userManager,
            JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // Finder brugeren ud fra email
            var user = await _userManager.FindByEmailAsync(dto.Email);

            // Hvis brugeren ikke findes
            if (user == null)
            {
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
                return Unauthorized(new
                {
                    message = "Forkert email eller adgangskode."
                });
            }

            // Opretter JWT-token til brugeren
            var token = _jwtService.CreateToken(user);

            // Sender token tilbage til MAUI-appen
            return Ok(new
            {
                token = token
            });
        }
    }
}