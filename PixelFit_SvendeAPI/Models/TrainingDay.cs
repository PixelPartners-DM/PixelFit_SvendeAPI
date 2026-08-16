namespace PixelFit_SvendeAPI.Models
{
    public class TrainingDay
    {
        public int Id { get; set; }

        // Det træningsprogram dagen tilhører
        public int TrainingProgramId { get; set; }

        public TrainingProgram Program { get; set; } = null!;

        // Den valgte dag i ugen
        public WeekDay DayName { get; set; }

        // De øvelser brugeren har valgt til denne træningsdag
        public List<TrainingDayExercise> Exercises { get; set; } = new();
    }
}