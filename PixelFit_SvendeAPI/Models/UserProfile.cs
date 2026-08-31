namespace PixelFit_SvendeAPI.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        // Brugerens oplysninger
        public string Gender { get; set; } = string.Empty;

        public int Age { get; set; }

        public int Height { get; set; }

        public double Weight { get; set; }

        // Fx. 1.2, 1.375, 1.55 osv.
        public double ActivityLevel { get; set; }

        // Beregnede værdier
        public double BMR { get; set; }

        public double TDEE { get; set; }

        // Det kaloriemål brugeren vælger
        public int DailyCalorieGoal { get; set; }
    }
}