using System.ComponentModel.DataAnnotations;

namespace restaurantAPI.DTO
{
    public class ProductBaseDto
    {
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public decimal Price { get; set; }

        
    }
    public class CreateProductDto : ProductBaseDto
    {
        [Required]
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

        public int Stock { get; set; }
        public int CategoryId { get; set; }  // allow changing category
        public CategoryDto? Category { get; set; } // navigation property for read-only
    }
}
