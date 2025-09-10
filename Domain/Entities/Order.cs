using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateOnly OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(CustomerName))
                throw new ArgumentException("Customer name cannot be empty.");
            if (OrderDetails == null || OrderDetails.Count == 0)
                throw new ArgumentException("Order must have at least one order detail.");
           // if (TotalAmount < 0)
             //   throw new ArgumentException("Total amount cannot be negative.");
        }
    }
}