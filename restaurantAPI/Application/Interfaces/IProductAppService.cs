using restaurantAPI.Domain.Entities;
using restaurantAPI.DTO;

namespace restaurantAPI.Application.Interfaces
{
    public interface IProductAppService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<(bool Success, string Message, int id)> AddAsync(CreateProductDto product);
        Task<(bool Success, string Message)> UpdateAsync(UpdateProductDto product);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}