namespace PixelFit_SvendeAPI.Models
{
    public class Meal
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public string MealType { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public List<FoodItem> Items { get; set; } = new();
    }
}