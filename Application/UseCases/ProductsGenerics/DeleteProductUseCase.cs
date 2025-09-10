using Application.Interfaces;
using Domain.Ports;
namespace Application.UseCases.ProductsGenerics
{
    public class DeleteProductUseCase<TModelProduct> : IDeleteProductUseCase<TModelProduct> where TModelProduct : DomainModel
    {
        private readonly IUnitOfWorkGeneric _unitOfWork;
        private readonly IDetailGenericRepository<TModelProduct> _productRepository;
        public DeleteProductUseCase(IUnitOfWorkGeneric unitOfWork, IDetailGenericRepository<TModelProduct> productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return (false, "Product does not exist.");
            }

            _productRepository.Remove(product);
            await _unitOfWork.CompleteAsync();

            return (true, "Product deleted successfully.");
        }
    }
}