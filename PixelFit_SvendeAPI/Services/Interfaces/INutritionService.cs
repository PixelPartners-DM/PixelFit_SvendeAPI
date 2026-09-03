using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.DTOS.Nutrition;

namespace PixelFit_SvendeAPI.Services.Interfaces
{
    public interface INutritionService
    {
        // Returns: computed DailyNutritionSummary, the user's DailyCalorieGoal, and the list of meals for today
        Task<(DailyNutritionSummary Summary, int DailyCalorieGoal, List<Meal> Meals)> GetTodayAsync(int userId);

        Task<Meal> CreateMealAsync(int userId, CreateMealRequest request);

        Task<bool> DeleteFoodItemAsync(int foodItemId, int userId);

        Task<bool> DeleteMealAsync(int mealId, int userId);
    }
}