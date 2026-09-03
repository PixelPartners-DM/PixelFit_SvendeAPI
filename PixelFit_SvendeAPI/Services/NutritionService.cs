using PixelFit_SvendeAPI.DTOS.Nutrition;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories.Interfaces;
using PixelFit_SvendeAPI.Services.Interfaces;

namespace PixelFit_SvendeAPI.Services
{
    public class NutritionService : INutritionService
    {
        private readonly INutritionRepository _repository;

        public NutritionService(INutritionRepository repository)
        {
            _repository = repository;
        }

        public async Task<(DailyNutritionSummary Summary, int DailyCalorieGoal, List<Meal> Meals)> GetTodayAsync(int userId)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var meals = await _repository.GetMealsForDateRangeAsync(userId, today, tomorrow);

            var totalCalories = meals.SelectMany(m => m.Items).Sum(i => i.Calories);
            var protein = meals.SelectMany(m => m.Items).Sum(i => i.Protein);
            var carbs = meals.SelectMany(m => m.Items).Sum(i => i.Carbs);
            var fat = meals.SelectMany(m => m.Items).Sum(i => i.Fat);
            var fiber = meals.SelectMany(m => m.Items).Sum(i => i.Fiber);

            var profile = await _repository.GetUserProfileByUserIdAsync(userId);
            var dailyGoal = profile?.DailyCalorieGoal ?? 0;
            var remainingCalories = dailyGoal > 0 ? Math.Max(0, dailyGoal - totalCalories) : 0;

            var summary = new DailyNutritionSummary
            {
                Date = today,
                TotalCalories = totalCalories,
                RemainingCalories = remainingCalories,
                Protein = protein,
                Carbs = carbs,
                Fat = fat,
                Fiber = fiber
            };

            return (summary, dailyGoal, meals);
        }

        public async Task<Meal> CreateMealAsync(int userId, CreateMealRequest request)
        {
            var meal = new Meal
            {
                UserId = userId,
                MealType = request.MealType,
                Date = request.Date ?? DateTime.Now,
                Items = request.Items.Select(item => new FoodItem
                {
                    Name = item.Name,
                    Calories = item.Calories,
                    Protein = item.Protein,
                    Carbs = item.Carbs,
                    Fat = item.Fat,
                    Fiber = item.Fiber
                }).ToList()
            };

            return await _repository.AddMealAsync(meal);
        }

        public async Task<bool> DeleteFoodItemAsync(int foodItemId, int userId)
        {
            var foodItem = await _repository.GetFoodItemWithMealAsync(foodItemId);

            if (foodItem == null)
                return false;

            if (foodItem.Meal.UserId != userId)
                return false;

            return await _repository.DeleteFoodItemAsync(foodItem);
        }

        public async Task<bool> DeleteMealAsync(int mealId, int userId)
        {
            var meal = await _repository.GetMealWithItemsAsync(mealId, userId);

            if (meal == null)
                return false;

            return await _repository.DeleteMealAsync(meal);
        }
    }
}