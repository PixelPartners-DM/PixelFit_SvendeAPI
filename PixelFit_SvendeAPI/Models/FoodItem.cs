namespace PixelFit_SvendeAPI.Models
{
    public class FoodItem
    {
        public int Id { get; set; }

        public int MealId { get; set; }

        public Meal Meal { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public int Calories { get; set; }

        public double Protein { get; set; }

        public double Carbs { get; set; }

        public double Fat { get; set; }

        public double Fiber { get; set; }
    }
}