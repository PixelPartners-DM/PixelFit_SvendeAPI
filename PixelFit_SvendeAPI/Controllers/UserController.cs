using Microsoft.AspNetCore.Mvc;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Services;

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

        // Opretter en ny bruger
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            // Tjekker om email allerede findes
            var existingUser = await _userService.FindByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                return Conflict(new
                {
                    message = "Emailen er allerede i brug."
                });
            }

            // Opretter brugerobjekt
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            // Sender bruger og adgangskode videre til servicen
            var createdUser = await _userService.CreateAsync(user, dto.Password);

            if (createdUser == null)
            {
                return BadRequest(new
                {
                    message = "Brugeren kunne ikke oprettes."
                });
            }

            // Sender kun nødvendige brugerdata tilbage
            var userDto = new UserDto
            {
                Id = createdUser.Id,
                Email = createdUser.Email ?? "",
                CreatedAt = createdUser.CreatedAt
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdUser.Id },
                userDto
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? "",
                CreatedAt = user.CreatedAt
            };

            return Ok(userDto);
        }
    }
}