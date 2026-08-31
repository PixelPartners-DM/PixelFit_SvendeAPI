namespace PixelFit_SvendeAPI.Models
{
    public class DailyNutritionSummary
    {
        public int Id { get; set; }

        // User bruger IdentityUser<int>, derfor skal UserId være int
        
        public int UserId { get; set; }


        //navigation property
        public User User { get; set; } = null!;

        // Datoen for registreringen
        public DateTime Date { get; set; }



        // Kalorier
        public int TotalCalories { get; set; }

        public int RemainingCalories { get; set; }




        // Makronæringsstoffer
        public double Protein { get; set; }

        public double Carbs { get; set; }

        public double Fat { get; set; }

        public double Fiber { get; set; }
    }
}