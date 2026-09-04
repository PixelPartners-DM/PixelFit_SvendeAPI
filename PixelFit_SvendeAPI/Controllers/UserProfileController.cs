using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Services.Interfaces;
using System.Security.Claims;

namespace PixelFit_SvendeAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _profileService;

        public UserProfileController(
            IUserProfileService profileService)
        {
            _profileService = profileService;
        }

        private int GetUserId()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier
                );

            return int.Parse(userId!);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetUserId();

            var profile = await _profileService.GetByUserIdAsync(userId);

            if (profile == null)
            {
                return NotFound(new
                {
                    message = "Der findes endnu ingen brugerprofil."
                });
            }

            return Ok(new
            {
                profile.Id,
                profile.Gender,
                profile.Age,
                profile.Height,
                profile.Weight,
                profile.ActivityLevel,
                profile.BMR,
                profile.TDEE,
                profile.DailyCalorieGoal
            });
        }

        [HttpPut("me")]
        public async Task<IActionResult> SaveMyProfile([FromBody] SaveUserProfileDto dto)
        {
            var userId = GetUserId();

            var profile = await _profileService.SaveAsync(userId, dto);

            return Ok(new
            {
                profile.Id,
                profile.Gender,
                profile.Age,
                profile.Height,
                profile.Weight,
                profile.ActivityLevel,
                profile.BMR,
                profile.TDEE,
                profile.DailyCalorieGoal
            });
        }
    }
}