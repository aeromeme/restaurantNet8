using Domain.Ports;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class OrderDetailModel: DomainModel
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual OrderModel? Order { get; set; }

    public virtual ProductModel? Product { get; set; }
}
