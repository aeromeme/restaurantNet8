using Domain.Entities;

namespace Domain.Ports
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithCategoryAsync();
        Task<Product?> GetByIdWithCategoryAsync(int id);
        Task<IEnumerable<Product>> GetByCategoryWithCategoryAsync(int categoryId); // Add this
    }
}
