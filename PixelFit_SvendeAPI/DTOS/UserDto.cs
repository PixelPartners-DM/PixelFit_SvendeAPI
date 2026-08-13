namespace PixelFit_SvendeAPI.DTOS
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}