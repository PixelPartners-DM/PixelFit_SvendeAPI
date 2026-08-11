using System;
using Microsoft.AspNetCore.Identity;

namespace PixelFit_SvendeAPI.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
