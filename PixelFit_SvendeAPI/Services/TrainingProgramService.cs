using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;
using PixelFit_SvendeAPI.Services.Interfaces;

namespace PixelFit_SvendeAPI.Services
{
    public class TrainingProgramService : ITrainingProgramService
    {
        private readonly ITrainingProgramRepository _repository;

        // Får repository gennem Dependency Injection
        public TrainingProgramService(
            ITrainingProgramRepository repository)
        {
            _repository = repository;
        }

        // Henter alle træningsprogrammer for en bruger
        public Task<IEnumerable<TrainingProgram>> GetByUserIdAsync(int userId)
        {
            return _repository.GetByUserIdAsync(userId);
        }

        // Henter ét bestemt træningsprogram
        public Task<TrainingProgram?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        // Opretter et nyt træningsprogram
        public Task<TrainingProgram> CreateAsync(TrainingProgram program)
        {
            return _repository.AddAsync(program);
        }

        // Opdaterer et eksisterende træningsprogram
        public Task<TrainingProgram> UpdateAsync(TrainingProgram program)
        {
            return _repository.UpdateAsync(program);
        }

        // Sletter et træningsprogram
        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}