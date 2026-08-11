namespace PixelFit_SvendeAPI.Models
{
    public class TrainingDay
    {
        public int Id { get; set; }

        public int TrainingProgramId { get; set; }
        public TrainingProgram Program { get; set; }

        public string DayName { get; set; }

        public List<Exercise> Exercises { get; set; } = new();
    }
}