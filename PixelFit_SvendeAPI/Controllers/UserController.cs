using Microsoft.AspNetCore.Mvc;
using PixelFit_SvendeAPI.Controllers;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Services;
using System.Threading.Tasks;

namespace PixelFit_SvendeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var existing = await _userService.FindByEmailAsync(dto.Email);
            if (existing != null) return Conflict(new { message = "Email already in use" });

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                Name = dto.Name
            };

            var created = await _userService.CreateAsync(user, dto.Password);
            if (created == null) return BadRequest(new { message = "User creation failed" });

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { id = created.Id, email = created.Email });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
