using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class OrderGenericRepository : Repository<OrderModel>, IDetailGenericRepository<OrderModel>
    {

        public OrderGenericRepository(RestaurantContext context) : base(context)
        {
        }
        public async Task<IEnumerable<OrderModel>> GetAllWithDetailAsync()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var data = await _context.Orders
                                 .Include(p => p.OrderDetails)  // single query, avoids N+1
                                 .ThenInclude(od => od.Product)  // include products in order details
                                 .ThenInclude(c => c.Category) // include category in products
                                 .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return data;
        }

        public async Task<OrderModel?> GetByIdWithDetailAsync(int id)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var data = await _context.Orders
                            .Where(p => p.OrderId == id)
                            .Include(p => p.OrderDetails)
                                .ThenInclude(od => od.Product)
                                    .ThenInclude(p => p.Category)
                            .FirstOrDefaultAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return data;

        }
    }
}
