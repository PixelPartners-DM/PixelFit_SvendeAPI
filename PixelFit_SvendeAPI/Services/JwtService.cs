using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Services
{
    // Service der opretter JWT-tokens til brugere
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        // Henter JWT-indstillinger fra appsettings.json
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Opretter et JWT-token til den bruger der er logget ind
        public string CreateToken(User user)
        {
            // Oplysninger der bliver gemt inde i tokenet
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            // Henter den hemmelige JWT-nøgle
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                )
            );

            // Bestemmer hvordan tokenet bliver signeret
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            // Opretter selve JWT-tokenet
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            // Konverterer tokenet til den tekststreng
            // som sendes tilbage til MAUI-appen
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}