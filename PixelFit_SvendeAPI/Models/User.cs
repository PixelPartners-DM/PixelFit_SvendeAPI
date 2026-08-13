using Microsoft.AspNetCore.Identity;

namespace PixelFit_SvendeAPI.Models
{
    public class User : IdentityUser<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}