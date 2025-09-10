using System.Linq.Expressions;
namespace Domain.Ports
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Remove(T entity);
        void Update(T entity);

        void Delete(T entity);
        Task SaveChanges();
    }
}
