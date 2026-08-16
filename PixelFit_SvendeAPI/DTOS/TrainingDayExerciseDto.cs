namespace PixelFit_SvendeAPI.DTOS
{
    public class TrainingDayExerciseDto
    {
        // Id på koblingen mellem træningsdag og øvelse
        public int Id { get; set; }

        // Id på træningsdagen
        public int TrainingDayId { get; set; }

        // Id på øvelsen
        public int ExerciseId { get; set; }

        // Navnet på øvelsen
        public string ExerciseName { get; set; } = string.Empty;

        // Billedsti som MAUI bruger til at vise øvelsen
        public string ImageUrl { get; set; } = string.Empty;

        // Pause efter øvelsen i sekunder
        public int RestBetweenExercises { get; set; }

        // Rækkefølgen på øvelsen på dagen
        public int Order { get; set; }
    }
}