using Application.DTO;
using Domain.Ports;

namespace Application.Interfaces
{
    public interface IUpdateProductUseCase<TCategoryModel,TModelProduct>
        where TCategoryModel : DomainModel
        where TModelProduct : DomainModel
    {
        Task<(bool Success, string Message)> ExecuteAsync(UpdateProductDto updateProductDto);
    }
}