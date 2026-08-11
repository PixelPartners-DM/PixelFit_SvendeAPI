namespace PixelFit_SvendeAPI.Models
{
    public class TrainingProgram
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Name { get; set; }

        public List<TrainingDay> Days { get; set; } = new();
    }
}