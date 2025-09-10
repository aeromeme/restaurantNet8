namespace Repository.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;
    using System;
    using Domain.Ports;
    using Repository.Models;

    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly RestaurantContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(RestaurantContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public async Task SaveChanges()=> await  _context.SaveChangesAsync();

        public void Delete(T entity)=> _dbSet.Remove(entity);
    }
}
