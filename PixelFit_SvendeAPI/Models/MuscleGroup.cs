namespace PixelFit_SvendeAPI.Models
{
    public class MuscleGroup
    {
        public int Id { get; set; }

        // Navnet på muskelgruppen
        // Fx. Bryst, Ryg eller Ben
        public string Name { get; set; } = string.Empty;

        // Alle øvelser som hører til muskelgruppen
        public List<Exercise> Exercises { get; set; } = new();
    }
}