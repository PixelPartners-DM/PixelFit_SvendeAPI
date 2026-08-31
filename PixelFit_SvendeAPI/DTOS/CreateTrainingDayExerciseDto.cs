using System.ComponentModel.DataAnnotations;

namespace PixelFit_SvendeAPI.DTOS
{
    public class CreateTrainingDayExerciseDto
    {
        // Id på den træningsdag øvelsen skal tilføjes til
        [Required]
        public int TrainingDayId { get; set; }

        // Id på øvelsen fra øvelsesbiblioteket
        [Required]
        public int ExerciseId { get; set; }

        // Pause efter øvelsen før næste øvelse
        public int RestBetweenExercises { get; set; }

        // Rækkefølgen på øvelsen på træningsdagen
        public int Order { get; set; }
    }
}