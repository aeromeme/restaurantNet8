using Domain.Entities;

namespace Application.Interfaces
{
    public interface IGetAllOrderUseCase<TOrderModel>
    {
        Task<IEnumerable<Order>> ExecuteAsync();
    }
}