using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Repositories.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByUserIdAsync(int userId);

        Task<UserProfile> AddAsync(UserProfile profile);

        Task<UserProfile> UpdateAsync(UserProfile profile);
    }
}