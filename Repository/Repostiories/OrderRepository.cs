using AutoMapper;
using Domain.Entities;
using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository.Repositories
{
    public class OrderRepository :  IOrderRepository
    {
        private readonly RestaurantContext _context;
        private readonly IMapper _map;

        public OrderRepository(RestaurantContext context, Mapper map) 
        {
            _context = context;
            _map = map;
        }

        

        public async Task<IEnumerable<Order>> GetAllWithDetailAsync()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var data= await _context.Orders
                                 .Include(p => p.OrderDetails)  // single query, avoids N+1
                                 .ThenInclude(od => od.Product)  // include products in order details
                                 .ThenInclude(p => p.Category) // include category in products
                                 .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var orders = _map.Map<List<Order>>(data);
            return orders;
        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var data = await _context.Orders
                                  .ToListAsync();

            var orders = _map.Map<List<Order>>(data);
            return orders;
        }



        public async Task<Order?> GetByIdWithDetailAsync(int id)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var data= await _context.Orders
                               .Include(p => p.OrderDetails)  // single query, avoids N+1
                               .ThenInclude(od => od.Product)  // include products in order details
                               .ThenInclude(p=> p.Category) // include category in products
                               .FirstOrDefaultAsync(p => p.OrderId == id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            var order =_map.Map<Order>(data);
            return order;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            var data = await _context.Orders
                              .FirstOrDefaultAsync(p => p.OrderId == id);
            var order = _map.Map<Order>(data);
            return order;
        }
        public async Task AddAsync(Order entity)
        {
            var data = _map.Map<OrderModel>(entity);
            await _context.Orders.AddAsync(data);
        }

        

        public void Remove(Order entity)
        {
            var ormData = _context.Orders.Find(entity.OrderId);
            if (ormData == null) return; // Fix: Only update if ormData is not null
            _context.Orders.Remove(ormData);
           
        }

        public void Update(Order entity)
        {
            var ormData = _context.Orders.Include(o => o.OrderDetails)
                                         .FirstOrDefault(o => o.OrderId == entity.OrderId);
            if (ormData == null) return; // Fix: Only update if ormData is not null

            _map.Map(entity, ormData);
            _context.Orders.Update(ormData);
        }

        public void Delete(Order entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
