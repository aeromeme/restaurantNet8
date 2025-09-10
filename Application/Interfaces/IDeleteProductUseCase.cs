using Domain.Ports;

namespace Application.Interfaces
{
    public interface IDeleteProductUseCase<TModelProduct>
        where TModelProduct : DomainModel
    {
        Task<(bool Success, string Message)> ExecuteAsync(int id);
    }
}