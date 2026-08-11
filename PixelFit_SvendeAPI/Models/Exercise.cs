namespace PixelFit_SvendeAPI.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        public int TrainingDayId { get; set; }
        public TrainingDay TrainingDay { get; set; }

        public string Name { get; set; }

        public int MuscleGroupId { get; set; }
        public MuscleGroup MuscleGroup { get; set; }

        public List<ExerciseSet> Sets { get; set; } = new();
    }
}