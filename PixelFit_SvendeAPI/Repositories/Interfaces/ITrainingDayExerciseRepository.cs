using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Repositories.Interfaces
{
    public interface ITrainingDayExerciseRepository
    {
        // Henter alle valgte øvelser til en bestemt træningsdag
        Task<IEnumerable<TrainingDayExercise>> GetByTrainingDayIdAsync(
            int trainingDayId);

        // Henter én bestemt valgt øvelse
        Task<TrainingDayExercise?> GetByIdAsync(int id);

        // Tilføjer en øvelse til en træningsdag
        Task<TrainingDayExercise> AddAsync(
            TrainingDayExercise trainingDayExercise);

        // Opdaterer en valgt øvelse
        Task<TrainingDayExercise> UpdateAsync(
            TrainingDayExercise trainingDayExercise);

        // Sletter en valgt øvelse fra træningsdagen
        Task<bool> DeleteAsync(int id);
    }
}