using Microsoft.AspNetCore.Mvc;

namespace restaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var cars = new[]
            {
                new
                {
                    id = 1,
                    make = "Toyota",
                    model = "Corolla",
                    year = 2022,
                    color = "Blue",
                    vin = "JTDBR32E720123456",
                    mileage = 15000,
                    price = 18500.00,
                    features = new[] { "Bluetooth", "Backup Camera", "Cruise Control" }
                },
                new
                {
                    id = 2,
                    make = "Honda",
                    model = "Civic",
                    year = 2021,
                    color = "Red",
                    vin = "2HGFC2F69MH123456",
                    mileage = 12000,
                    price = 19500.00,
                    features = new[] { "Heated Seats", "Apple CarPlay", "Lane Assist" }
                },
                new
                {
                    id = 3,
                    make = "Ford",
                    model = "Focus",
                    year = 2020,
                    color = "White",
                    vin = "1FADP3F20LL123456",
                    mileage = 20000,
                    price = 17000.00,
                    features = new[] { "Navigation", "Remote Start", "Parking Sensors" }
                },
                new
                {
                    id = 4,
                    make = "Chevrolet",
                    model = "Malibu",
                    year = 2019,
                    color = "Black",
                    vin = "1G1ZD5STXKF123456",
                    mileage = 25000,
                    price = 16000.00,
                    features = new[] { "Android Auto", "Blind Spot Monitor", "Keyless Entry" }
                },
                new
                {
                    id = 5,
                    make = "Hyundai",
                    model = "Elantra",
                    year = 2023,
                    color = "Silver",
                    vin = "KMHD84LF6PU123456",
                    mileage = 5000,
                    price = 21000.00,
                    features = new[] { "Sunroof", "Wireless Charging", "Adaptive Cruise Control" }
                }
            };

            return Ok(cars);
        }

        public class CarInput
        {
            public string Make { get; set; }
            public string Model { get; set; }
            public int Year { get; set; }
            public string Color { get; set; }
            public string Vin { get; set; }
            public int Mileage { get; set; }
            public double Price { get; set; }
            public string[] Features { get; set; }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CarInput car)
        {
            // Simulate assigning a new ID (e.g., next available)
            var createdCar = new
            {
                id = Random.Shared.Next(6, 1001), // Simulated new ID random 6 to 1000
                make = car.Make,
                model = car.Model,
                year = car.Year,
                color = car.Color,
                vin = car.Vin,
                mileage = car.Mileage,
                price = car.Price,
                features = car.Features
            };

            // Simulate CreatedAtAction (would normally return the route to the new resource)
            return CreatedAtAction(nameof(Get), new { id = createdCar.id }, createdCar);
        }
    }
}