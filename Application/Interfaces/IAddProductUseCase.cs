using Application.DTO;
using Domain.Ports;

namespace Application.Interfaces
{
    public interface IAddProductUseCase<TCategory, TProduct>
        where TCategory : DomainModel
        where TProduct : DomainModel
    {
        Task<(bool Success, string Message, int id)> ExecuteAsync(CreateProductDto createProductDto);
    }
}