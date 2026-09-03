using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelFit_SvendeAPI.DTOS.Nutrition;
using PixelFit_SvendeAPI.Services.Interfaces;
using System.Security.Claims;

namespace PixelFit_SvendeAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionController : ControllerBase
    {
        // Dependency injection of the INutritionService
        private readonly INutritionService _nutritionService;

        public NutritionController(
            INutritionService nutritionService)
        {
            _nutritionService = nutritionService;
        }

        private int GetUserId()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier
                );

            return int.Parse(userId!);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetToday()
        {
            var userId = GetUserId();
            var (summary, dailyGoal, meals) = await _nutritionService.GetTodayAsync(userId);

            var result = new
            {
                Date = summary.Date,
                DailyCalorieGoal = dailyGoal,
                TotalCalories = summary.TotalCalories,
                RemainingCalories = summary.RemainingCalories,
                Protein = summary.Protein,
                Carbs = summary.Carbs,
                Fat = summary.Fat,
                Fiber = summary.Fiber,
                Meals = meals.Select(meal => new
                {
                    meal.Id,
                    meal.MealType,
                    meal.Date,
                    Items = meal.Items.Select(item => new
                    {
                        item.Id,
                        item.Name,
                        item.Calories,
                        item.Protein,
                        item.Carbs,
                        item.Fat,
                        item.Fiber
                    })
                })
            };

            return Ok(result);
        }

        [HttpPost("meals")]
        public async Task<IActionResult> CreateMeal(
            [FromBody] CreateMealRequest request)
        {
            var userId = GetUserId();

            if (string.IsNullOrWhiteSpace(request.MealType))
            {
                return BadRequest(new
                {
                    message = "Måltidstype mangler."
                });
            }

            if (request.Items == null || request.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message = "Måltidet skal indeholde mindst én madvare."
                });
            }

            var meal = await _nutritionService.CreateMealAsync(userId, request);

            return Ok(new
            {
                meal.Id,
                meal.MealType,
                meal.Date,
                Items = meal.Items.Select(item => new
                {
                    item.Id,
                    item.Name,
                    item.Calories,
                    item.Protein,
                    item.Carbs,
                    item.Fat,
                    item.Fiber
                })
            });
        }

        [HttpDelete("food/{foodItemId:int}")]
        public async Task<IActionResult> DeleteFoodItem(int foodItemId)
        {
            var userId = GetUserId();

            var ok = await _nutritionService.DeleteFoodItemAsync(foodItemId, userId);

            if (!ok)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("meals/{mealId:int}")]
        public async Task<IActionResult> DeleteMeal(int mealId)
        {
            var userId = GetUserId();

            var ok = await _nutritionService.DeleteMealAsync(mealId, userId);

            if (!ok)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}


