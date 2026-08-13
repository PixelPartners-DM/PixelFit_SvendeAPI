using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Services
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(int id);

        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> CreateAsync(User user, string password);

        Task<User> UpdateAsync(User user);

        Task<bool> DeleteAsync(int id);

        Task<User?> FindByEmailAsync(string email);
    }
}