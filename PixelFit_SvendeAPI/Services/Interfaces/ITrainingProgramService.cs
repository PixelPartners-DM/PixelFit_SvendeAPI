using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Services.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<IEnumerable<TrainingProgram>> GetByUserIdAsync(int userId);

        Task<TrainingProgram?> GetByIdAsync(int id);

        Task<TrainingProgram> CreateAsync(TrainingProgram program);

        Task<TrainingProgram> UpdateAsync(TrainingProgram program);

        Task<bool> DeleteAsync(int id);
    }
}