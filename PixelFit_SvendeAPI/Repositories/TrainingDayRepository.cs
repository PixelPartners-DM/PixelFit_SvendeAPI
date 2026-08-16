using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;

namespace PixelFit_SvendeAPI.Repositories
{
    public class TrainingDayRepository : ITrainingDayRepository
    {
        private readonly ApplicationDbContext _context;

        // Får adgang til databasen gennem Dependency Injection
        public TrainingDayRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        // Henter alle træningsdage som tilhører et bestemt program
        public async Task<IEnumerable<TrainingDay>> GetByProgramIdAsync(
            int trainingProgramId)
        {
            return await _context.TrainingDays
                .Where(day => day.TrainingProgramId == trainingProgramId)
                .AsNoTracking()
                .ToListAsync();
        }


        // Henter én bestemt træningsdag ud fra id
        public async Task<TrainingDay?> GetByIdAsync(int id)
        {
            return await _context.TrainingDays
                .FirstOrDefaultAsync(day => day.Id == id);
        }


        // Opretter en ny træningsdag
        public async Task<TrainingDay> AddAsync(
            TrainingDay trainingDay)
        {
            await _context.TrainingDays.AddAsync(trainingDay);

            await _context.SaveChangesAsync();

            return trainingDay;
        }


        // Opdaterer en eksisterende træningsdag
        public async Task<TrainingDay> UpdateAsync(
            TrainingDay trainingDay)
        {
            _context.TrainingDays.Update(trainingDay);

            await _context.SaveChangesAsync();

            return trainingDay;
        }


        // Sletter en træningsdag
        public async Task<bool> DeleteAsync(int id)
        {
            var trainingDay =
                await _context.TrainingDays.FindAsync(id);

            if (trainingDay == null)
            {
                return false;
            }

            _context.TrainingDays.Remove(trainingDay);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}