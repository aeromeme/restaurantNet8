using System;
using System.Collections.Generic;

namespace restaurantAPI.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; } = null!;

    public DateOnly OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
