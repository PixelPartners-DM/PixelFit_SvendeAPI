namespace PixelFit_SvendeAPI.Models
{
    public class TrainingProgram
    {
        public int Id { get; set; }

        // ID på den bruger som ejer programmet
        public int UserId { get; set; }

        // Navigation property til brugeren
        public User User { get; set; } = null!;

        // Navnet på træningsprogrammet
        public string Name { get; set; } = string.Empty;

        // Træningsdage som hører til programmet
        public List<TrainingDay> Days { get; set; } = new();
    }
}