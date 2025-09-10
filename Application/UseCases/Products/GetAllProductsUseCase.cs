using AutoMapper;
using Domain.Ports;
using Domain.Entities;
using Application.DTO;

namespace Application.UseCases.Products
{
    public class GetAllProductsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProductsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product>> ExecuteAsync()
        {
            var products = await _unitOfWork.Products.GetAllWithCategoryAsync();
            return _mapper.Map<List<Product>>(products);
        }
    }
}