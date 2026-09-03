using PixelFit_SvendeAPI.Models;
using System;
namespace PixelFit_SvendeAPI.Repositories.Interfaces
{
    public interface INutritionRepository
    {
        Task<List<Meal>> GetMealsForDateRangeAsync(int userId, DateTime from, DateTime to);

        Task<UserProfile?> GetUserProfileByUserIdAsync(int userId);

        Task<FoodItem?> GetFoodItemWithMealAsync(int foodItemId);

        Task<Meal?> GetMealWithItemsAsync(int mealId, int userId);

        Task<Meal> AddMealAsync(Meal meal);

        Task<bool> DeleteFoodItemAsync(FoodItem foodItem);

        Task<bool> DeleteMealAsync(Meal meal);
    }
}