namespace PixelFit_SvendeAPI.Models
{
    public class WeightEntry
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime Date { get; set; }
        public double Weight { get; set; }
    }
}
