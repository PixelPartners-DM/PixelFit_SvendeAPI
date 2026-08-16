using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;
using PixelFit_SvendeAPI.Services.Interfaces;

namespace PixelFit_SvendeAPI.Services
{
    public class ExerciseSetService : IExerciseSetService
    {
        private readonly IExerciseSetRepository _repository;

        // Får repository gennem Dependency Injection
        public ExerciseSetService(
            IExerciseSetRepository repository)
        {
            _repository = repository;
        }


        // Henter alle sæt til en bestemt valgt øvelse
        public Task<IEnumerable<ExerciseSet>>
            GetByTrainingDayExerciseIdAsync(
                int trainingDayExerciseId)
        {
            return _repository
                .GetByTrainingDayExerciseIdAsync(
                    trainingDayExerciseId
                );
        }


        // Henter ét bestemt sæt
        public Task<ExerciseSet?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }


        // Opretter et nyt sæt
        public Task<ExerciseSet> AddAsync(
            ExerciseSet exerciseSet)
        {
            return _repository.AddAsync(
                exerciseSet
            );
        }


        // Opdaterer et eksisterende sæt
        public Task<ExerciseSet> UpdateAsync(
            ExerciseSet exerciseSet)
        {
            return _repository.UpdateAsync(
                exerciseSet
            );
        }


        // Sletter et sæt
        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}