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


        /// Henter ernæringsoversigten for i dag for en given bruger.
        /// Parameter bestående af:
        /// - DailyNutritionSummary: sammendrag af dagens ernæring,
        /// - DailyCalorieGoal: brugerens daglige kaloriemål,
        /// - Meals: listen af måltider for i dag.
        public async Task<(DailyNutritionSummary Summary, int DailyCalorieGoal, List<Meal> Meals)> GetTodayAsync(int userId)
        {
            // Fastlæg dagens start (midnat) så vi får hele dagen fra 00:00
            var today = DateTime.Today;

            // Beregn starten af næste dag for at lave et eksklusivt interval [today, tomorrow)
            var tomorrow = today.AddDays(1);

            // Hent alle måltider for brugeren i det givne datointerval
            var meals = await _repository.GetMealsForDateRangeAsync(userId, today, tomorrow);

            // Afregner og summer næringsdata fra alle måltidernes items
            // Brug SelectMany for at flade alle items ud fra hver meal til én sekvens
            var totalCalories = meals.SelectMany(m => m.Items).Sum(i => i.Calories); // Samlede kalorier for dagen
            var protein = meals.SelectMany(m => m.Items).Sum(i => i.Protein); // Samlet protein (g)
            var carbs = meals.SelectMany(m => m.Items).Sum(i => i.Carbs); // Samlet kulhydrat (g)
            var fat = meals.SelectMany(m => m.Items).Sum(i => i.Fat); // Samlet fedt (g)
            var fiber = meals.SelectMany(m => m.Items).Sum(i => i.Fiber); // Samlet fiber (g)

            // Forsøg at hent brugerens profil (kan være null hvis ikke sat)
            var profile = await _repository.GetUserProfileByUserIdAsync(userId);

            // Dagligt kaloriemål; hvis profil mangler, sættes det til 0
            var dailyGoal = profile?.DailyCalorieGoal ?? 0;

            // Beregn tilbageværende kalorier kun hvis der er et positivt mål
            // Brug Math.Max for at undgå negative rester (min 0)
            var remainingCalories = dailyGoal > 0 ? Math.Max(0, dailyGoal - totalCalories) : 0;

            // Sammensæt oversigten med alle beregnede værdier
            var summary = new DailyNutritionSummary
            {
                Date = today, // Dato for denne oversigt (dagens dato)
                TotalCalories = totalCalories, // Samlede kalorier indtaget i dag
                RemainingCalories = remainingCalories, // Hvor mange kalorier der er tilbage af målet
                Protein = protein, // Samlet protein indtaget
                Carbs = carbs, // Samlet kulhydrat indtaget
                Fat = fat, // Samlet fedt indtaget
                Fiber = fiber // Samlet fiber indtaget
            };

            // Returner tuple: oversigt, brugerens daglige mål og de hentede måltider
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