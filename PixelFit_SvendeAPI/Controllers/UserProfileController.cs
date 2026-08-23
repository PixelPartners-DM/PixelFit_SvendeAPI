using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;
using System.Security.Claims;

namespace PixelFit_SvendeAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserProfileController(
            ApplicationDbContext context)
        {
            _context = context;
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
            var userId =
                GetUserId();


            var profile =
                await _context.UserProfiles
                    .FirstOrDefaultAsync(
                        x => x.UserId == userId
                    );


            if (profile == null)
            {
                return NotFound(new
                {
                    message =
                        "Der findes endnu ingen brugerprofil."
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
        public async Task<IActionResult> SaveMyProfile(
            [FromBody] SaveUserProfileDto dto)
        {
            var userId =
                GetUserId();


            var profile =
                await _context.UserProfiles
                    .FirstOrDefaultAsync(
                        x => x.UserId == userId
                    );


            if (profile == null)
            {
                profile =
                    new UserProfile
                    {
                        UserId =
                            userId,

                        Gender =
                            dto.Gender,

                        Age =
                            dto.Age,

                        Height =
                            dto.Height,

                        Weight =
                            dto.Weight,

                        ActivityLevel =
                            dto.ActivityLevel,

                        BMR =
                            dto.BMR,

                        TDEE =
                            dto.TDEE,

                        DailyCalorieGoal =
                            dto.DailyCalorieGoal
                    };


                await _context.UserProfiles.AddAsync(
                    profile
                );
            }


            else
            {
                profile.Gender =
                    dto.Gender;

                profile.Age =
                    dto.Age;

                profile.Height =
                    dto.Height;

                profile.Weight =
                    dto.Weight;

                profile.ActivityLevel =
                    dto.ActivityLevel;

                profile.BMR =
                    dto.BMR;

                profile.TDEE =
                    dto.TDEE;

                profile.DailyCalorieGoal =
                    dto.DailyCalorieGoal;
            }


            await _context.SaveChangesAsync();


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