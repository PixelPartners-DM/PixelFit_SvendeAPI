using System.ComponentModel.DataAnnotations;

namespace PixelFit_SvendeAPI.DTOS
{
    public class CreateTrainingProgramDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
