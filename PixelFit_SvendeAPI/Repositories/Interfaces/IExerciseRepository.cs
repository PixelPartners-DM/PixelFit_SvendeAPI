using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Repositories.Interfaces
{
    public interface IExerciseRepository
    {
        // Henter alle øvelser
        Task<IEnumerable<Exercise>> GetAllAsync();

        // Henter én bestemt øvelse
        Task<Exercise?> GetByIdAsync(int id);

        // Henter alle øvelser for en bestemt muskelgruppe
        Task<IEnumerable<Exercise>> GetByMuscleGroupIdAsync(int muscleGroupId);
    }
}