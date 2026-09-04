using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;

namespace PixelFit_SvendeAPI.Repositories
{
    public class NutritionRepository : INutritionRepository
    {
        private readonly ApplicationDbContext _context;

        public NutritionRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        /// Hent alle måltider for en bruger inden for et givet datointerval.
        /// "userId" Brugerens unikke ID.
        /// "from" Startdato for intervallet (inklusiv).
        /// "to" Slutdato for intervallet (eksklusiv).
        /// En liste af måltider inden for det specificerede datointerval bliver returneret, sorteret efter dato.
        /// Hvert måltid inkluderer relaterede FoodItems.
        // Eager-load'er relaterede FoodItems, filtrerer på bruger og dato, sorterer og returnerer som liste.
        public async Task<List<Meal>> GetMealsForDateRangeAsync(int userId, DateTime from, DateTime to)
        {
            // Eager-load Items; filtrer på bruger og datointerval (from inkl., to ekskl.);
            // sorter efter dato og udfør som read-only med AsNoTracking.
            return await _context.Meals
                .Include(m => m.Items)
                .Where(m => m.UserId == userId && m.Date >= from && m.Date < to)
                .OrderBy(m => m.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UserProfile?> GetUserProfileByUserIdAsync(int userId)
        {
            return await _context.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<FoodItem?> GetFoodItemWithMealAsync(int foodItemId)
        {
            return await _context.FoodItems
                .Include(fi => fi.Meal)
                .FirstOrDefaultAsync(fi => fi.Id == foodItemId);
        }

        public async Task<Meal?> GetMealWithItemsAsync(int mealId, int userId)
        {
            return await _context.Meals
                .Include(m => m.Items)
                .FirstOrDefaultAsync(m => m.Id == mealId && m.UserId == userId);
        }

        public async Task<Meal> AddMealAsync(Meal meal)
        {
            await _context.Meals.AddAsync(meal);
            await _context.SaveChangesAsync();

            return meal;
        }

        public async Task<bool> DeleteFoodItemAsync(FoodItem foodItem)
        {
            _context.FoodItems.Remove(foodItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteMealAsync(Meal meal)
        {
            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}