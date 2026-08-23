namespace PixelFit_SvendeAPI.DTOS
{
    public class SaveUserProfileDto
    {
        public string Gender { get; set; } =
            string.Empty;

        public int Age { get; set; }

        public int Height { get; set; }

        public double Weight { get; set; }

        public double ActivityLevel { get; set; }

        public double BMR { get; set; }

        public double TDEE { get; set; }

        public int DailyCalorieGoal { get; set; }
    }
}