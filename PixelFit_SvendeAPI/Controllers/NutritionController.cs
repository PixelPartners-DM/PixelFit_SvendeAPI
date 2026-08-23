using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.Models;
using System.Security.Claims;
using PixelFit_SvendeAPI.DTOS.Nutrition;

namespace PixelFit_SvendeAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NutritionController(
            ApplicationDbContext context)
        {
            _context = context;
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
            var userId =
                GetUserId();


            var today =
                DateTime.Today;


            var tomorrow =
                today.AddDays(1);


            var meals =
                await _context.Meals
                    .Include(meal => meal.Items)
                    .Where(
                        meal =>
                            meal.UserId == userId &&
                            meal.Date >= today &&
                            meal.Date < tomorrow
                    )
                    .OrderBy(meal => meal.Date)
                    .ToListAsync();


            var totalCalories =
                meals
                    .SelectMany(meal => meal.Items)
                    .Sum(item => item.Calories);


            var protein =
                meals
                    .SelectMany(meal => meal.Items)
                    .Sum(item => item.Protein);


            var carbs =
                meals
                    .SelectMany(meal => meal.Items)
                    .Sum(item => item.Carbs);


            var fat =
                meals
                    .SelectMany(meal => meal.Items)
                    .Sum(item => item.Fat);


            var fiber =
                meals
                    .SelectMany(meal => meal.Items)
                    .Sum(item => item.Fiber);


            // Henter brugerens gemte kaloriemål
            var profile =
                await _context.UserProfiles
                    .FirstOrDefaultAsync(
                        profile =>
                            profile.UserId == userId
                    );


            var dailyGoal =
                profile?.DailyCalorieGoal ?? 0;


            var remainingCalories =
                dailyGoal > 0
                    ? Math.Max(
                        0,
                        dailyGoal - totalCalories
                    )
                    : 0;


            var result = new
            {
                Date =
                    today,

                DailyCalorieGoal =
                    dailyGoal,

                TotalCalories =
                    totalCalories,

                RemainingCalories =
                    remainingCalories,

                Protein =
                    protein,

                Carbs =
                    carbs,

                Fat =
                    fat,

                Fiber =
                    fiber,

                Meals =
                    meals.Select(
                        meal => new
                        {
                            meal.Id,

                            meal.MealType,

                            meal.Date,

                            Items =
                                meal.Items.Select(
                                    item => new
                                    {
                                        item.Id,

                                        item.Name,

                                        item.Calories,

                                        item.Protein,

                                        item.Carbs,

                                        item.Fat,

                                        item.Fiber
                                    }
                                )
                        }
                    )
            };


            return Ok(result);
        }


        [HttpPost("meals")]
        public async Task<IActionResult> CreateMeal(
            [FromBody] CreateMealRequest request)
        {
            var userId =
                GetUserId();


            if (string.IsNullOrWhiteSpace(
                request.MealType))
            {
                return BadRequest(new
                {
                    message =
                        "Måltidstype mangler."
                });
            }


            if (request.Items == null ||
                request.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message =
                        "Måltidet skal indeholde mindst én madvare."
                });
            }


            var meal =
                new Meal
                {
                    UserId =
                        userId,

                    MealType =
                        request.MealType,

                    Date =
                        request.Date ??
                        DateTime.Now,

                    Items =
                        request.Items.Select(
                            item =>
                                new FoodItem
                                {
                                    Name =
                                        item.Name,

                                    Calories =
                                        item.Calories,

                                    Protein =
                                        item.Protein,

                                    Carbs =
                                        item.Carbs,

                                    Fat =
                                        item.Fat,

                                    Fiber =
                                        item.Fiber
                                }
                        )
                        .ToList()
                };


            await _context.Meals.AddAsync(
                meal
            );


            await _context.SaveChangesAsync();


            return Ok(new
            {
                meal.Id,

                meal.MealType,

                meal.Date,

                Items =
                    meal.Items.Select(
                        item => new
                        {
                            item.Id,

                            item.Name,

                            item.Calories,

                            item.Protein,

                            item.Carbs,

                            item.Fat,

                            item.Fiber
                        }
                    )
            });
        }



        [HttpDelete("food/{foodItemId:int}")]
        public async Task<IActionResult> DeleteFoodItem(
            int foodItemId)
        {
            var userId =
                GetUserId();


            var foodItem =
                await _context.FoodItems
                    .Include(
                        item => item.Meal
                    )
                    .FirstOrDefaultAsync(
                        item =>
                            item.Id ==
                            foodItemId
                    );


            if (foodItem == null)
            {
                return NotFound();
            }


            // Brugeren må kun slette egne foods
            if (foodItem.Meal.UserId != userId)
            {
                return NotFound();
            }


            _context.FoodItems.Remove(
                foodItem
            );


            await _context.SaveChangesAsync();


            return NoContent();
        }



        [HttpDelete("meals/{mealId:int}")]
        public async Task<IActionResult> DeleteMeal(
            int mealId)
        {
            var userId =
                GetUserId();


            var meal =
                await _context.Meals
                    .Include(
                        meal => meal.Items
                    )
                    .FirstOrDefaultAsync(
                        meal =>
                            meal.Id == mealId &&
                            meal.UserId == userId
                    );


            if (meal == null)
            {
                return NotFound();
            }


            _context.Meals.Remove(
                meal
            );


            await _context.SaveChangesAsync();


            return NoContent();
        }
    }
}


