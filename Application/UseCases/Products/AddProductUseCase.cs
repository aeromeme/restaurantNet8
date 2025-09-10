using AutoMapper;
using Domain.Ports;
using Domain.Entities;
using Application.DTO;

namespace Application.UseCases.Products
{
    public class AddProductUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddProductUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(bool Success, string Message, int id)> ExecuteAsync(CreateProductDto createProductDto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(createProductDto.CategoryId);
            if (category == null)
                return (false, "Category does not exist.", 0);

            var product = _mapper.Map<Product>(createProductDto);

            try
            {
                product.Validate();
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message, 0);
            }

          

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            return (true, "Product created successfully.", product.ProductId);
        }
    }

}
