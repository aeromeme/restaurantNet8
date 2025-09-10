using Application.Tools;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTO
{

    public class OrderBaseDto
    {
        [Required]
        public string CustomerName { get; set; } = null!;
        [Required]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly OrderDate { get; set; }

       
    }
    public class OrderDetailBaseDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
    public class CreateOrderDto : OrderBaseDto
    { public virtual ICollection<CreateOrderDetailDto> OrderDetails { get; set; } = new List<CreateOrderDetailDto>(); }
    public class CreateOrderDetailDto : OrderDetailBaseDto
    {
    }
     public class OrderDto : OrderBaseDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }

        public virtual ICollection<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }
    public class OrderDetailDto : OrderDetailBaseDto
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public ProductDto? Product { get; set; } // <-- Make nullable

        public decimal Amount => UnitPrice * Quantity;
    }
}
