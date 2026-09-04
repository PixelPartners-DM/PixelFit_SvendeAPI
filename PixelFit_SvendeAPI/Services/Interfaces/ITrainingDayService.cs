using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Services.Interfaces
{
    public interface ITrainingDayService
    {
        // Henter alle træningsdage for et bestemt program
        Task<IEnumerable<TrainingDay>> GetByProgramIdAsync(int trainingProgramId);

        // Henter én bestemt træningsdag
        Task<TrainingDay?> GetByIdAsync(int id);

        // Opretter en ny træningsdag
        Task<TrainingDay> CreateAsync(TrainingDay trainingDay);

        // Opdaterer en eksisterende træningsdag
        Task<TrainingDay> UpdateAsync(TrainingDay trainingDay);

        // Sletter en træningsdag
        Task<bool> DeleteAsync(int id);

        // Tjekker om brugeren allerede har en træningsdag
        // med den angivne ugedag. excludeId kan bruges ved update.
        Task<bool> DayAlreadyExistsForUserAsync(WeekDay dayName, int userId, int? excludeId = null);
    }
}