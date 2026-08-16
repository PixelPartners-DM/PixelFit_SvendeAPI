namespace PixelFit_SvendeAPI.DTOS
{
    public class MuscleGroupDto
    {
        // Id på muskelgruppen
        public int Id { get; set; }

        // Fx. Bryst, Ryg eller Ben
        public string Name { get; set; } = string.Empty;
    }
}