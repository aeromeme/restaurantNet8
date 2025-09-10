using Domain.Entities;
using Application.DTO;

namespace Application.Interfaces
{
    public interface IOrderAppService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<(bool Success, string Message, int? NewOrderId)> AddAsync(CreateOrderDto order);
        Task<(bool Success, string Message)> UpdateAsync(OrderDto order);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
