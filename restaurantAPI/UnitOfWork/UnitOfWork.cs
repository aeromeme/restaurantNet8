using restaurantAPI.Models;
using restaurantAPI.Repostiories;
using System;

namespace restaurantAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RestaurantContext _context;

        public IProductRepository Products { get; }
        public IRepository<Category> Categories { get; }
        public OrderRepository Orders { get; }
        public IRepository<OrderDetail> OrderDetails { get; }

        public UnitOfWork(RestaurantContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
            Categories = new Repository<Category>(_context);
            Orders = new OrderRepository(_context);
            OrderDetails = new Repository<OrderDetail>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
