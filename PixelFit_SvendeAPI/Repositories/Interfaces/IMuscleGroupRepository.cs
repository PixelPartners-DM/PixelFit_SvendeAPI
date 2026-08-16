using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Repositories.Interfaces
{
    public interface IMuscleGroupRepository
    {
        // Henter alle muskelgrupper
        Task<IEnumerable<MuscleGroup>> GetAllAsync();

        // Henter én bestemt muskelgruppe
        Task<MuscleGroup?> GetByIdAsync(int id);
    }
}