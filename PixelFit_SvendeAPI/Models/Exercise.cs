namespace PixelFit_SvendeAPI.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        // Navnet på øvelsen
        // Fx. "Bench Press"
        public string Name { get; set; } = string.Empty;

        // Hvilken muskelgruppe øvelsen tilhører
        public int MuscleGroupId { get; set; }

        public MuscleGroup MuscleGroup { get; set; } = null!;

        // Stien til billedet som vises i MAUI
        public string ImageUrl { get; set; } = string.Empty;
    }
}