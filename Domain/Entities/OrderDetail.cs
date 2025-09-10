using System;

namespace Domain.Entities
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Product? Product { get; set; }

        public void Validate()
        {
            if (Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            if (UnitPrice < 0)
                throw new ArgumentException("Unit price cannot be negative.");
        }
    }
}