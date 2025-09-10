using Repository.Models;
using Repository.Repositories;
using System;
using AutoMapper;
using Domain.Ports;
using Domain.Entities;

namespace Respository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RestaurantContext _context;

        public IProductRepository Products { get; }
        public IRepository<Category> Categories { get; }
        public IOrderRepository Orders { get; }
 

        public UnitOfWork(RestaurantContext context, Mapper _map)
        {
            _context = context;
            Products = new ProductRepository(_context,_map);
            Categories = new CategoryRepository(_context,_map);
            Orders = new OrderRepository(_context,_map);
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
