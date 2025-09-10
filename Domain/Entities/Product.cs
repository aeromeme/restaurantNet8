namespace Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        // Navigation property for Category
        public Category? Category { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ProductName))
                throw new ArgumentException("Product name cannot be empty.");

            if (Price < 0)
                throw new ArgumentException("Price cannot be negative.");
        }
    }
}