namespace restaurantAPI.DTO
{
    public class ProductBaseDto
    {
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
    public class CreateProductDto : ProductBaseDto
    {
        public int CategoryId { get; set; }  // foreign key to Category
    }
    public class UpdateProductDto : ProductBaseDto
    {
        public int Id { get; set; }          // ID of the product to update
        public int CategoryId { get; set; }  // allow changing category
    }

    public class ProductDto : ProductBaseDto
    {
        public int Id { get; set; }
        public CategoryDto? Category { get; set; } // navigation property for read-only
    }
}
