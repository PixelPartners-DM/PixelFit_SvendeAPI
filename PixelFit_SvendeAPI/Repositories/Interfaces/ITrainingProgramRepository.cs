using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Repositories.Interfaces
{
    public interface ITrainingProgramRepository
    {
        Task<IEnumerable<TrainingProgram>> GetByUserIdAsync(int userId);

        Task<TrainingProgram?> GetByIdAsync(int id);

        Task<TrainingProgram> AddAsync(TrainingProgram program);

        Task<TrainingProgram> UpdateAsync(TrainingProgram program);

        Task<bool> DeleteAsync(int id);
    }
}