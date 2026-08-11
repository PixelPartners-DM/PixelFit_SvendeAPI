namespace PixelFit_SvendeAPI.Models
{
    public class Meal
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string MealType { get; set; } // Morgenmad, Frokost, Aftensmad, Snack
        public DateTime Date { get; set; }

        public List<FoodItem> Items { get; set; } = new();
    }
}
