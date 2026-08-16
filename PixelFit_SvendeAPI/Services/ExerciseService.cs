using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;
using PixelFit_SvendeAPI.Services.Interfaces;

namespace PixelFit_SvendeAPI.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _repository;


        // Får repository gennem Dependency Injection
        public ExerciseService(
            IExerciseRepository repository)
        {
            _repository = repository;
        }


        // Henter alle øvelser
        public Task<IEnumerable<Exercise>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }


        // Henter én bestemt øvelse
        public Task<Exercise?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }


        // Henter alle øvelser for en bestemt muskelgruppe
        public Task<IEnumerable<Exercise>> GetByMuscleGroupIdAsync(
            int muscleGroupId)
        {
            return _repository.GetByMuscleGroupIdAsync(
                muscleGroupId
            );
        }
    }
}