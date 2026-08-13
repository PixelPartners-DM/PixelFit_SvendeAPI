using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);

        Task<IEnumerable<User>> GetAllAsync();

        Task<User> AddAsync(User user);

        Task<User> UpdateAsync(User user);

        Task<bool> DeleteAsync(int id);

        Task<User?> FindByEmailAsync(string email);
    }
}