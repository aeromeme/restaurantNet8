using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ProductGenericRepository : Repository<ProductModel>, IDetailGenericRepository<ProductModel>
    {
       
        public ProductGenericRepository(RestaurantContext context):base(context)
        {
        }
        public async Task<IEnumerable<ProductModel>> GetAllWithDetailAsync()
        {
            var data = await _context.Products
                               .Include(p => p.Category) 
                               .ToListAsync();
            return data;
        }

        public async Task<ProductModel?> GetByIdWithDetailAsync(int id)
        {
            var data = await _context.Products
                                .Where(p => p.ProductId == id)
                               .Include(p => p.Category)
                               .FirstOrDefaultAsync();
            return data;

        }
    }
}
