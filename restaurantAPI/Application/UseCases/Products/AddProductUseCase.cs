using AutoMapper;
using restaurantAPI.UnitOfWork;
using restaurantAPI.Domain.Entities;
using restaurantAPI.DTO;

namespace restaurantAPI.Application.Products.UseCases
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

            var product = _mapper.Map<ProductEntity>(createProductDto);

            try
            {
                product.Validate();
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message, 0);
            }

            var ormProduct = _mapper.Map<restaurantAPI.Models.Product>(product);

            await _unitOfWork.Products.AddAsync(ormProduct);
            await _unitOfWork.CompleteAsync();

            return (true, "Product created successfully.", ormProduct.ProductId);
        }
    }

}
