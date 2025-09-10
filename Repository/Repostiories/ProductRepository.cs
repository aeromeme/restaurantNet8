using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Ports;
using Repository.Models;
using System;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Repository.Repositories
{
    public class ProductRepository  : IProductRepository
    {
        private readonly RestaurantContext _context;
        private readonly IMapper _map;

        public ProductRepository(RestaurantContext context, Mapper map) 
        {
            _context = context;
            _map = map;
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        {
            var data= await _context.Products
                                 .Include(p => p.Category)  // single query, avoids N+1
                                 .ToListAsync();
            var products= _map.Map<List<Product>>(data);
            return products;
        }

        public async Task<Product?> GetByIdWithCategoryAsync(int id)
        {
            var data= await _context.Products
                                 .Include(p => p.Category)
                                 .FirstOrDefaultAsync(p => p.ProductId == id);
            var product =_map.Map<Product>(data) ;
            return product;
        }

        public async Task<IEnumerable<Product>> GetByCategoryWithCategoryAsync(int categoryId)
        {
            var data= await _context.Products
                                 .Include(p => p.Category)
                                 .Where(p => p.CategoryId == categoryId)
                                 .ToListAsync();
            var products = _map.Map<List<Product>>(data);
            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var data = await _context.Products.FindAsync(id);
            var product = _map.Map<Product>(data);
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var data = await _context.Products.ToListAsync();
            var products = _map.Map<List<Product>>(data);
            return products;
        }


        public async Task AddAsync(Product entity)
        {
           var data = _map.Map<ProductModel>(entity);
           await _context.Products.AddAsync(data);
        }

        public void Remove(Product entity)
        {
            var ormData = _context.Products.Find(entity.ProductId);
            if (ormData == null) return; // Fix: Only update if ormData is not null
            _context.Products.Remove(ormData);
        }

        public void Update(Product entity)
        {
            var ormData = _context.Products.Find(entity.ProductId);
            if (ormData == null) return; // Fix: Only update if ormData is not null
            _map.Map(entity, ormData);
            _context.Products.Update(ormData);
        }

        public void Delete(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
