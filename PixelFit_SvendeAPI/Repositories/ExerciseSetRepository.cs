using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;

namespace PixelFit_SvendeAPI.Repositories
{
    public class ExerciseSetRepository : IExerciseSetRepository
    {
        private readonly ApplicationDbContext _context;

        // Får adgang til databasen gennem Dependency Injection
        public ExerciseSetRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }


        // Henter alle sæt til en bestemt valgt øvelse
        public async Task<IEnumerable<ExerciseSet>>
            GetByTrainingDayExerciseIdAsync(
                int trainingDayExerciseId)
        {
            return await _context.ExerciseSets
                .Where(set =>
                    set.TrainingDayExerciseId ==
                    trainingDayExerciseId)
                .AsNoTracking()
                .ToListAsync();
        }


        // Henter ét bestemt sæt
        public async Task<ExerciseSet?>
            GetByIdAsync(int id)
        {
            return await _context.ExerciseSets
                .FirstOrDefaultAsync(
                    set => set.Id == id
                );
        }


        // Opretter et nyt sæt
        public async Task<ExerciseSet> AddAsync(
            ExerciseSet exerciseSet)
        {
            await _context.ExerciseSets.AddAsync(
                exerciseSet
            );

            await _context.SaveChangesAsync();

            return exerciseSet;
        }


        // Opdaterer et eksisterende sæt
        public async Task<ExerciseSet> UpdateAsync(
            ExerciseSet exerciseSet)
        {
            _context.ExerciseSets.Update(
                exerciseSet
            );

            await _context.SaveChangesAsync();

            return exerciseSet;
        }


        // Sletter et sæt
        public async Task<bool> DeleteAsync(int id)
        {
            var exerciseSet =
                await _context.ExerciseSets
                    .FirstOrDefaultAsync(
                        set => set.Id == id
                    );

            if (exerciseSet == null)
            {
                return false;
            }

            _context.ExerciseSets.Remove(
                exerciseSet
            );

            await _context.SaveChangesAsync();

            return true;
        }
    }
}