using PixelFit_SvendeAPI.Controllers;

namespace PixelFit_SvendeAPI.DTOS.Nutrition
{
    public class CreateMealRequest
    {
        public string MealType { get; set; } =
            string.Empty;

        public DateTime? Date { get; set; }

        public List<CreateFoodItemRequest> Items { get; set; } =
            new();
    }
}