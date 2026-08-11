namespace PixelFit_SvendeAPI.Models
{
    public class TrainingHistory
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime Date { get; set; }

        public string MuscleGroups { get; set; } // fx "Bryst, Triceps"
        public int TotalWeightLifted { get; set; }
        public int DurationMinutes { get; set; }
    }
}
