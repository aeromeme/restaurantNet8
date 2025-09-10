using AutoMapper;
using Domain.Ports;
using Domain.Entities;
using Application.DTO;

namespace Application.UseCases.Products
{
    public class GetProductByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Product?> ExecuteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);
            return product == null ? null : _mapper.Map<Product>(product);
        }
    }
}