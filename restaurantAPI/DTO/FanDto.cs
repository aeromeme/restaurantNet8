namespace restaurantAPI.DTO
{
    public class CreateFanDto
    {

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int YearsAsFan { get; set; }
    }
}
