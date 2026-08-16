using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;
using PixelFit_SvendeAPI.Services.Interfaces;

namespace PixelFit_SvendeAPI.Services
{
    public class MuscleGroupService : IMuscleGroupService
    {
        private readonly IMuscleGroupRepository _repository;


        // Får repository gennem Dependency Injection
        public MuscleGroupService(
            IMuscleGroupRepository repository)
        {
            _repository = repository;
        }


        // Henter alle muskelgrupper
        public Task<IEnumerable<MuscleGroup>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }


        // Henter én bestemt muskelgruppe
        public Task<MuscleGroup?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }
    }
}