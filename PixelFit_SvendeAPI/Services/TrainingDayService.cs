using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;
using PixelFit_SvendeAPI.Services.Interfaces;

namespace PixelFit_SvendeAPI.Services
{
    public class TrainingDayService : ITrainingDayService
    {
        private readonly ITrainingDayRepository _repository;

        // Får repository gennem Dependency Injection
        public TrainingDayService(
            ITrainingDayRepository repository)
        {
            _repository = repository;
        }


        // Henter alle træningsdage for et bestemt program
        public Task<IEnumerable<TrainingDay>> GetByProgramIdAsync(
            int trainingProgramId)
        {
            return _repository.GetByProgramIdAsync(trainingProgramId);
        }


        // Henter én bestemt træningsdag
        public Task<TrainingDay?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }


        // Opretter en ny træningsdag
        public Task<TrainingDay> CreateAsync(
            TrainingDay trainingDay)
        {
            return _repository.AddAsync(trainingDay);
        }


        // Opdaterer en træningsdag
        public Task<TrainingDay> UpdateAsync(
            TrainingDay trainingDay)
        {
            return _repository.UpdateAsync(trainingDay);
        }


        // Sletter en træningsdag
        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}