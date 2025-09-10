
using Domain.Entities;
namespace Domain.Ports
{


    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IRepository<Category> Categories { get; }
        IOrderRepository Orders { get; }

        Task<int> CompleteAsync();
    }
}
