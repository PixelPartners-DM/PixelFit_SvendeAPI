namespace PixelFit_SvendeAPI.DTOS
{
    public class ExerciseDto
    {
        // Id på øvelsen
        public int Id { get; set; }

        // Navnet på øvelsen
        // Fx. Bench Press
        public string Name { get; set; } = string.Empty;

        // Id på muskelgruppen øvelsen tilhører
        public int MuscleGroupId { get; set; }

        // Navnet på muskelgruppen
        // Fx. Bryst
        public string MuscleGroupName { get; set; } = string.Empty;

        // Billede som MAUI kan vise ved øvelsen
        public string ImageUrl { get; set; } = string.Empty;
    }
}