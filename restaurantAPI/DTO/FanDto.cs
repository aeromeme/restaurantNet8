using System.ComponentModel.DataAnnotations;

namespace restaurantAPI.DTO
{
    public class CreateFanDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public int YearsAsFan { get; set; }
    }
}
