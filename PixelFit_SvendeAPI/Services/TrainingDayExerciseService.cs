using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;
using PixelFit_SvendeAPI.Services.Interfaces;

namespace PixelFit_SvendeAPI.Services
{
    public class TrainingDayExerciseService
        : ITrainingDayExerciseService
    {
        private readonly ITrainingDayExerciseRepository _repository;

        // Får repository gennem Dependency Injection
        public TrainingDayExerciseService(
            ITrainingDayExerciseRepository repository)
        {
            _repository = repository;
        }


        // Henter alle valgte øvelser
        // til en bestemt træningsdag
        public Task<IEnumerable<TrainingDayExercise>>
            GetByTrainingDayIdAsync(int trainingDayId)
        {
            return _repository.GetByTrainingDayIdAsync(
                trainingDayId
            );
        }


        // Henter én bestemt valgt øvelse
        public Task<TrainingDayExercise?>
            GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }


        // Tilføjer en øvelse til en træningsdag
        public Task<TrainingDayExercise> AddAsync(
            TrainingDayExercise trainingDayExercise)
        {
            return _repository.AddAsync(
                trainingDayExercise
            );
        }


        // Opdaterer en valgt øvelse
        public Task<TrainingDayExercise> UpdateAsync(
            TrainingDayExercise trainingDayExercise)
        {
            return _repository.UpdateAsync(
                trainingDayExercise
            );
        }


        // Fjerner en øvelse fra træningsdagen
        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}