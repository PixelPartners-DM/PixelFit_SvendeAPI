namespace PixelFit_SvendeAPI.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string Gender { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string ActivityLevel { get; set; }

        public double BMR { get; set; }
        public double TDEE { get; set; }
    }
}