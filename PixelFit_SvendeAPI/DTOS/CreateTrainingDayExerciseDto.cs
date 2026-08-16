using System.ComponentModel.DataAnnotations;

namespace PixelFit_SvendeAPI.DTOS
{
    public class CreateTrainingDayExerciseDto
    {
        // Id på den træningsdag øvelsen skal tilføjes til
        [Required]
        public int TrainingDayId { get; set; }

        // Id på øvelsen fra øvelsesbiblioteket
        // Fx. Bench Press
        [Required]
        public int ExerciseId { get; set; }

        // Pause efter øvelsen før næste øvelse
        // Angives i sekunder
        public int RestBetweenExercises { get; set; }

        // Rækkefølgen på øvelsen på træningsdagen
        // Fx. 1 = første øvelse
        public int Order { get; set; }
    }
}