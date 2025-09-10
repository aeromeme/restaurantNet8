using Domain.Ports;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class OrderModel: DomainModel
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; } = null!;

    public DateOnly OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public virtual ICollection<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
}
