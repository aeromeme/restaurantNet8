using Domain.Entities;
using Domain.Ports;

namespace Application.Interfaces
{
    public interface IGetAllProductsUseCase<TModelProduct>
        where TModelProduct : DomainModel
    {
        Task<IEnumerable<Product>> ExecuteAsync();
    }
}