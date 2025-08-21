using restaurantAPI.Models;

namespace restaurantAPI.Repostiories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllWithDetailAsync();
        Task<Order?> GetByIdWithDetailAsync(int id);
    }
}
