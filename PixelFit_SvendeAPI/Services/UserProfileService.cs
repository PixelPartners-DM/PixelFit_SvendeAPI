using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;
using PixelFit_SvendeAPI.Services.Interfaces;

namespace PixelFit_SvendeAPI.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _repository;

        public UserProfileService(IUserProfileRepository repository)
        {
            _repository = repository;
        }

        public Task<UserProfile?> GetByUserIdAsync(int userId)
        {
            return _repository.GetByUserIdAsync(userId);
        }

        public async Task<UserProfile> SaveAsync(int userId, SaveUserProfileDto dto)
        {
            var profile = await _repository.GetByUserIdAsync(userId);

            if (profile == null)
            {
                profile = new UserProfile
                {
                    UserId = userId,
                    Gender = dto.Gender,
                    Age = dto.Age,
                    Height = dto.Height,
                    Weight = dto.Weight,
                    ActivityLevel = dto.ActivityLevel,
                    BMR = dto.BMR,
                    TDEE = dto.TDEE,
                    DailyCalorieGoal = dto.DailyCalorieGoal
                };

                return await _repository.AddAsync(profile);
            }

            profile.Gender = dto.Gender;
            profile.Age = dto.Age;
            profile.Height = dto.Height;
            profile.Weight = dto.Weight;
            profile.ActivityLevel = dto.ActivityLevel;
            profile.BMR = dto.BMR;
            profile.TDEE = dto.TDEE;
            profile.DailyCalorieGoal = dto.DailyCalorieGoal;

            return await _repository.UpdateAsync(profile);
        }
    }
}