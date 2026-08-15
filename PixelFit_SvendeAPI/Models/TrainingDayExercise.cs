namespace PixelFit_SvendeAPI.Models
{
    // Repræsenterer en øvelse som brugeren
    // har valgt til en bestemt træningsdag
    public class TrainingDayExercise
    {
        public int Id { get; set; }


        // Den træningsdag øvelsen tilhører
        public int TrainingDayId { get; set; }

        public TrainingDay TrainingDay { get; set; } = null!;


        // Den valgte øvelse fra øvelsesbiblioteket
        public int ExerciseId { get; set; }

        public Exercise Exercise { get; set; } = null!;


        // Pause efter øvelsen før næste øvelse starter
        public int RestBetweenExercises { get; set; }


        // Bestemmer rækkefølgen på øvelserne
        // Fx. 1 = Bænkpres, 2 = Cable Flyes
        public int Order { get; set; }


        // De sæt brugeren har valgt til øvelsen
        public List<ExerciseSet> Sets { get; set; } = new();
    }
}