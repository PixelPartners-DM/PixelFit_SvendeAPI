namespace PixelFit_SvendeAPI.DTOS.Nutrition
{
    public class CreateFoodItemRequest
    {
        public string Name { get; set; } =
            string.Empty;

        public int Calories { get; set; }

        public double Protein { get; set; }

        public double Carbs { get; set; }

        public double Fat { get; set; }

        public double Fiber { get; set; }
    }
}