using PixelFit_SvendeAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace PixelFit_SvendeAPI.DTOS
{
    public class CreateTrainingDayDto
    {
        // Id på det træningsprogram dagen skal tilhøre
        [Required]
        public int TrainingProgramId { get; set; }

        // Brugeren skal vælge én af ugens dage
        [Required]
        public WeekDay? DayName { get; set; }
    }
}