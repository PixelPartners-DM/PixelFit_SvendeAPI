namespace PixelFit_SvendeAPI.Models
{
    public class DailyNutritionSummary
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime Date { get; set; }

        public int TotalCalories { get; set; }
        public int RemainingCalories { get; set; }

        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public double Fiber { get; set; }
    }
}
