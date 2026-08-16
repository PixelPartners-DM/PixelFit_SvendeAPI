using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;

namespace PixelFit_SvendeAPI.Repositories
{
    public class TrainingDayExerciseRepository
        : ITrainingDayExerciseRepository
    {
        private readonly ApplicationDbContext _context;

        // Får adgang til databasen gennem Dependency Injection
        public TrainingDayExerciseRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }


        // Henter alle øvelser som er valgt
        // til en bestemt træningsdag
        public async Task<IEnumerable<TrainingDayExercise>>
            GetByTrainingDayIdAsync(int trainingDayId)
        {
            return await _context.TrainingDayExercises

                // Henter information om selve øvelsen
                // fx. Bench Press + ImageUrl
                .Include(x => x.Exercise)

                // Henter også sættene til øvelsen
                .Include(x => x.Sets)

                // Kun øvelser til den valgte træningsdag
                .Where(x =>
                    x.TrainingDayId == trainingDayId)

                // Sorterer efter den rækkefølge brugeren har valgt
                .OrderBy(x => x.Order)

                .ToListAsync();
        }


        // Henter én bestemt valgt øvelse
        public async Task<TrainingDayExercise?>
            GetByIdAsync(int id)
        {
            return await _context.TrainingDayExercises

                .Include(x => x.Exercise)

                .Include(x => x.Sets)

                .FirstOrDefaultAsync(x =>
                    x.Id == id);
        }


        // Tilføjer en øvelse til en træningsdag
        public async Task<TrainingDayExercise> AddAsync(
            TrainingDayExercise trainingDayExercise)
        {
            await _context.TrainingDayExercises.AddAsync(
                trainingDayExercise
            );

            await _context.SaveChangesAsync();

            return trainingDayExercise;
        }


        // Opdaterer en valgt øvelse
        public async Task<TrainingDayExercise> UpdateAsync(
            TrainingDayExercise trainingDayExercise)
        {
            _context.TrainingDayExercises.Update(
                trainingDayExercise
            );

            await _context.SaveChangesAsync();

            return trainingDayExercise;
        }


        // Fjerner en øvelse fra træningsdagen
        public async Task<bool> DeleteAsync(int id)
        {
            var trainingDayExercise =
                await _context.TrainingDayExercises
                    .FirstOrDefaultAsync(x =>
                        x.Id == id);

            if (trainingDayExercise == null)
            {
                return false;
            }

            _context.TrainingDayExercises.Remove(
                trainingDayExercise
            );

            await _context.SaveChangesAsync();

            return true;
        }
    }
}