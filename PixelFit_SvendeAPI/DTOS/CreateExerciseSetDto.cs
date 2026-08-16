using System.ComponentModel.DataAnnotations;

namespace PixelFit_SvendeAPI.DTOS
{
    public class CreateExerciseSetDto
    {
        // Id på den valgte øvelse i træningsdagen
        [Required]
        public int TrainingDayExerciseId { get; set; }

        // Antal gentagelser
        [Range(1, 100)]
        public int Reps { get; set; }

        // Vægt i kg
        [Range(0, 1000)]
        public double Weight { get; set; }

        // Pause mellem sættene i sekunder
        [Range(0, 3600)]
        public int RestBetweenSets { get; set; }
    }
}