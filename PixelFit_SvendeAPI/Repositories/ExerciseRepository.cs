using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;

namespace PixelFit_SvendeAPI.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly ApplicationDbContext _context;

        // Får adgang til databasen gennem Dependency Injection
        public ExerciseRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        // Henter alle øvelser fra databasen
        public async Task<IEnumerable<Exercise>> GetAllAsync()
        {
            return await _context.Exercises
                .Include(exercise => exercise.MuscleGroup)
                .AsNoTracking()
                .ToListAsync();
        }


        // Henter én bestemt øvelse ud fra id
        public async Task<Exercise?> GetByIdAsync(int id)
        {
            return await _context.Exercises
                .Include(exercise => exercise.MuscleGroup)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    exercise => exercise.Id == id
                );
        }


        // Henter alle øvelser for en bestemt muskelgruppe
        public async Task<IEnumerable<Exercise>> GetByMuscleGroupIdAsync(
            int muscleGroupId)
        {
            return await _context.Exercises
                .Include(exercise => exercise.MuscleGroup)
                .Where(exercise =>
                    exercise.MuscleGroupId == muscleGroupId
                )
                .AsNoTracking()
                .ToListAsync();
        }
    }
}