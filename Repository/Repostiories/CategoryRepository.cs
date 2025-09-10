using AutoMapper;
using Domain.Entities;
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
    public class CategoryRepository : IRepository<Category>
    {
        private readonly RestaurantContext _context;
        private readonly IMapper _map;
        public CategoryRepository( RestaurantContext context, Mapper map) {
            _context = context;
            _map = map;
        }
        public Task AddAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Category entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var data = await _context.Categories 
                                 .ToListAsync();
            var categories = _map.Map<List<Category>>(data);
            return categories;
        }

        public Task<Category?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
