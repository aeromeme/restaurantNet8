using Domain.Ports;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class ProductModel: DomainModel
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public virtual CategoryModel? Category { get; set; }

    public virtual ICollection<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
}
