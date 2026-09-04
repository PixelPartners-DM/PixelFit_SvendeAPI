using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.DTOS.Nutrition;

namespace PixelFit_SvendeAPI.Services.Interfaces
{
    public interface INutritionService
    {
        // Returnerer: computed DailyNutritionSummary, brugerens DailyCalorieGoal og listen over måltider for i dag
        Task<(DailyNutritionSummary Summary, int DailyCalorieGoal, List<Meal> Meals)> GetTodayAsync(int userId);

        Task<Meal> CreateMealAsync(int userId, CreateMealRequest request);

        Task<bool> DeleteFoodItemAsync(int foodItemId, int userId);

        Task<bool> DeleteMealAsync(int mealId, int userId);
    }
}