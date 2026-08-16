using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Services.Interfaces
{
    public interface IExerciseService
    {
        // Henter alle øvelser
        Task<IEnumerable<Exercise>> GetAllAsync();

        // Henter én bestemt øvelse
        Task<Exercise?> GetByIdAsync(int id);

        // Henter alle øvelser for en bestemt muskelgruppe
        Task<IEnumerable<Exercise>> GetByMuscleGroupIdAsync(
            int muscleGroupId);
    }
}