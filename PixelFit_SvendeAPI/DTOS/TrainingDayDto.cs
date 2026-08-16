using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.DTOS
{
    public class TrainingDayDto
    {
        public int Id { get; set; }

        public int TrainingProgramId { get; set; }

        public WeekDay DayName { get; set; }
    }
}