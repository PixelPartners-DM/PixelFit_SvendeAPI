namespace PixelFit_SvendeAPI.DTOS
{
    public class ExerciseSetDto
    {
        // Id på selve sættet
        public int Id { get; set; }

        // Den valgte øvelse sættet tilhører
        public int TrainingDayExerciseId { get; set; }

        // Antal gentagelser
        public int Reps { get; set; }

        // Vægt i kg
        public double Weight { get; set; }

        // Pause efter sættet i sekunder
        public int RestBetweenSets { get; set; }
    }
}