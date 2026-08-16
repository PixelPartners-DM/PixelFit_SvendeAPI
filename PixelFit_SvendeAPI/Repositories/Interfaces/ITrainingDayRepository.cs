using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Repositories.Interfaces
{
    public interface ITrainingDayRepository
    {
        // Henter alle træningsdage for et bestemt program
        Task<IEnumerable<TrainingDay>> GetByProgramIdAsync(int trainingProgramId);

        // Henter én bestemt træningsdag
        Task<TrainingDay?> GetByIdAsync(int id);

        // Opretter en ny træningsdag
        Task<TrainingDay> AddAsync(TrainingDay trainingDay);

        // Opdaterer en eksisterende træningsdag
        Task<TrainingDay> UpdateAsync(TrainingDay trainingDay);

        // Sletter en træningsdag
        Task<bool> DeleteAsync(int id);
    }
}