using System.ComponentModel.DataAnnotations;

namespace PixelFit_SvendeAPI.DTOS
{
    public class LoginDto
    {
        // Email skal være udfyldt og have et gyldigt email-format
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Adgangskoden skal være udfyldt
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}