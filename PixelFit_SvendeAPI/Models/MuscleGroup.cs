namespace PixelFit_SvendeAPI.Models
{
    public class MuscleGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Exercise> Exercises { get; set; } = new();
    }
}
