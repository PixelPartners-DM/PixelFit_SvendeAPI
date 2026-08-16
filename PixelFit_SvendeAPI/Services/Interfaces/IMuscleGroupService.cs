using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Services.Interfaces
{
    public interface IMuscleGroupService
    {
        // Henter alle muskelgrupper
        Task<IEnumerable<MuscleGroup>> GetAllAsync();

        // Henter én bestemt muskelgruppe
        Task<MuscleGroup?> GetByIdAsync(int id);
    }
}