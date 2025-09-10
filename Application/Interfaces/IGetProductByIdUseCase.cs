using Domain.Entities;

using Domain.Ports;
namespace Application.Interfaces
{
    public interface IGetProductByIdUseCase<TModelProduct>
        where TModelProduct : DomainModel
    {
        Task<Product?> ExecuteAsync(int id);
    }
}