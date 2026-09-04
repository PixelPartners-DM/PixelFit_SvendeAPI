using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfile?> GetByUserIdAsync(int userId);

        Task<UserProfile> SaveAsync(int userId, SaveUserProfileDto dto);
    }
}