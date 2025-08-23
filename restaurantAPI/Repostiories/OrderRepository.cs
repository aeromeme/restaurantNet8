using Microsoft.EntityFrameworkCore;
using restaurantAPI.Models;

namespace restaurantAPI.Repostiories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly RestaurantContext _context;

        public OrderRepository(RestaurantContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllWithDetailAsync()
        {
            return await _context.Orders
                                 .Include(p => p.OrderDetails)  // single query, avoids N+1
                                 .ThenInclude(od => od.Product)  // include products in order details
                                 .ThenInclude(p => p.Category) // include category in products
                                 .ToListAsync();
        }

        public async Task<Order?> GetByIdWithDetailAsync(int id)
        {
            return await _context.Orders
                               .Include(p => p.OrderDetails)  // single query, avoids N+1
                               .ThenInclude(od => od.Product)  // include products in order details
                               .ThenInclude(p=> p.Category) // include category in products
                               .FirstOrDefaultAsync(p => p.OrderId == id);
        }
    }
}
