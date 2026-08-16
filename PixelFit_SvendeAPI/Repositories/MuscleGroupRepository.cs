using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;

namespace PixelFit_SvendeAPI.Repositories
{
    public class MuscleGroupRepository : IMuscleGroupRepository
    {
        private readonly ApplicationDbContext _context;

        // Får adgang til databasen gennem Dependency Injection
        public MuscleGroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        // Henter alle muskelgrupper fra databasen
        public async Task<IEnumerable<MuscleGroup>> GetAllAsync()
        {
            return await _context.MuscleGroups
                .AsNoTracking()
                .ToListAsync();
        }


        // Henter én bestemt muskelgruppe ud fra id
        public async Task<MuscleGroup?> GetByIdAsync(int id)
        {
            return await _context.MuscleGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    muscleGroup => muscleGroup.Id == id
                );
        }
    }
}