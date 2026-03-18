namespace restaurantAPI.Domain.Entities
{
    public class Fan
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; }= null!;

        public int YearsAsFan { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email))
            {
                return false;
            }
            if (YearsAsFan < 0 || YearsAsFan > 100)
            {
                return false;
            }
            return true;
        }

    }
}
