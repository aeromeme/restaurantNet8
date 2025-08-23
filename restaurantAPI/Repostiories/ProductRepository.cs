using Microsoft.EntityFrameworkCore;
using restaurantAPI.Models;
using System;

namespace restaurantAPI.Repostiories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly RestaurantContext _context;

        public ProductRepository(RestaurantContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        {
            return await _context.Products
                                 .Include(p => p.Category)  // single query, avoids N+1
                                 .ToListAsync();
        }

        public async Task<Product?> GetByIdWithCategoryAsync(int id)
        {
            return await _context.Products
                                 .Include(p => p.Category)
                                 .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> GetByCategoryWithCategoryAsync(int categoryId)
        {
            return await _context.Products
                                 .Include(p => p.Category)
                                 .Where(p => p.CategoryId == categoryId)
                                 .ToListAsync();
        }
    }
}
