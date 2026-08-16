using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Repositories.Interfaces
{
    public interface IExerciseSetRepository
    {
        // Henter alle sæt til en bestemt valgt øvelse
        Task<IEnumerable<ExerciseSet>> GetByTrainingDayExerciseIdAsync(
            int trainingDayExerciseId);

        // Henter ét bestemt sæt
        Task<ExerciseSet?> GetByIdAsync(int id);

        // Opretter et nyt sæt
        Task<ExerciseSet> AddAsync(
            ExerciseSet exerciseSet);

        // Opdaterer et eksisterende sæt
        Task<ExerciseSet> UpdateAsync(
            ExerciseSet exerciseSet);

        // Sletter et sæt
        Task<bool> DeleteAsync(int id);
    }
}