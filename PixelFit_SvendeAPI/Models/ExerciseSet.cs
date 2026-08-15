namespace PixelFit_SvendeAPI.Models
{
    public class ExerciseSet
    {
        public int Id { get; set; }

        // Den valgte øvelse på en bestemt træningsdag
        public int TrainingDayExerciseId { get; set; }

        public TrainingDayExercise TrainingDayExercise { get; set; } = null!;

        // Antal gentagelser i sættet
        public int Reps { get; set; }

        // Vægten der løftes i sættet
        public double Weight { get; set; }

        // Pause mellem sættene i sekunder
        public int RestBetweenSets { get; set; }
    }
}