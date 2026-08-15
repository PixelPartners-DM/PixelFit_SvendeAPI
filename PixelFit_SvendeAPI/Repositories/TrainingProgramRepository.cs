using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;

namespace PixelFit_SvendeAPI.Repositories
{
    public class TrainingProgramRepository : ITrainingProgramRepository
    {
        private readonly ApplicationDbContext _context;

        // Får adgang til databasen gennem Dependency Injection
        public TrainingProgramRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        // Henter alle træningsprogrammer for én bestemt bruger
        public async Task<IEnumerable<TrainingProgram>> GetByUserIdAsync(int userId)
        {
            return await _context.TrainingPrograms
                .Where(program => program.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }


        // Henter ét bestemt træningsprogram ud fra id
        public async Task<TrainingProgram?> GetByIdAsync(int id)
        {
            return await _context.TrainingPrograms
                .FirstOrDefaultAsync(program => program.Id == id);
        }


        // Opretter et nyt træningsprogram
        public async Task<TrainingProgram> AddAsync(TrainingProgram program)
        {
            await _context.TrainingPrograms.AddAsync(program);

            await _context.SaveChangesAsync();

            return program;
        }


        // Opdaterer et eksisterende træningsprogram
        public async Task<TrainingProgram> UpdateAsync(TrainingProgram program)
        {
            _context.TrainingPrograms.Update(program);

            await _context.SaveChangesAsync();

            return program;
        }


        // Sletter et træningsprogram
        public async Task<bool> DeleteAsync(int id)
        {
            var program = await _context.TrainingPrograms.FindAsync(id);

            if (program == null)
            {
                return false;
            }

            _context.TrainingPrograms.Remove(program);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}