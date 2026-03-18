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
        public IRepository<Order> Orders { get; }
        
        public IOrderRepository OrderQuery { get; }
        public IRepository<OrderDetail> OrderDetails { get; }


        public UnitOfWork(RestaurantContext context, IProductRepository productRepository, IRepository<Category> categoryRepository, IRepository<Order> orderRepository,IOrderRepository orderQueryRepository, IRepository<OrderDetail> orderDetailRepository)
        {
            _context = context;
            Products = productRepository;
            Categories = categoryRepository;
            Orders = orderRepository;
            OrderQuery = orderQueryRepository;
            OrderDetails = orderDetailRepository;
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
