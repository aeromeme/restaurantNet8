using Domain.Ports;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class CategoryModel: DomainModel
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
}
