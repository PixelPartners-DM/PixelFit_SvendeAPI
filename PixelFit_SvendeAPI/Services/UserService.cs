using Microsoft.AspNetCore.Identity;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;
using PixelFit_SvendeAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using PixelFit_SvendeAPI.Repositories;

namespace PixelFit_SvendeAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public async Task<User> CreateAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) return null;
            // reload from store to ensure populated fields
            return await _userManager.FindByIdAsync(user.Id.ToString());
        }

        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<User>> GetAllAsync() => _repo.GetAllAsync();

        public Task<User> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<User> UpdateAsync(User user) => _repo.UpdateAsync(user);

        public Task<User> FindByEmailAsync(string email) => _repo.FindByEmailAsync(email);
    }
}