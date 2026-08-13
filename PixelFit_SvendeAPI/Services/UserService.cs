using Microsoft.AspNetCore.Identity;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;

namespace PixelFit_SvendeAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly UserManager<User> _userManager;

        public UserService(
            IUserRepository repo,
            UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        // Opretter en ny bruger
        public async Task<User?> CreateAsync(User user, string password)
        {
            // Identity opretter brugeren og hasher adgangskoden
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return null;
            }

            // Henter brugeren igen efter oprettelse
            return await _userManager.FindByIdAsync(
                user.Id.ToString()
            );
        }

        // Finder bruger ud fra email
        public Task<User?> FindByEmailAsync(string email)
        {
            return _repo.FindByEmailAsync(email);
        }

        // Henter bruger ud fra id
        public Task<User?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        // Henter alle brugere
        public Task<IEnumerable<User>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }

        // Opdaterer bruger
        public Task<User> UpdateAsync(User user)
        {
            return _repo.UpdateAsync(user);
        }

        // Sletter bruger
        public Task<bool> DeleteAsync(int id)
        {
            return _repo.DeleteAsync(id);
        }
    }
}