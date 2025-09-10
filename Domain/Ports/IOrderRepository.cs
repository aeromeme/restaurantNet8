using Domain.Entities;

namespace Domain.Ports
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllWithDetailAsync();
        Task<Order?> GetByIdWithDetailAsync(int id);
    }
}
