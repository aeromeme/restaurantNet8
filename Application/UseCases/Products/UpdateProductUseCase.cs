using AutoMapper;
using Domain.Ports;
using Domain.Entities;
using Application.DTO;

namespace Application.UseCases.Products
{
    public class UpdateProductUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProductUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(UpdateProductDto updateProductDto)
        {
            var ormProduct = await _unitOfWork.Products.GetByIdAsync(updateProductDto.Id);
            if (ormProduct == null)
                return (false, "Product does not exist.");

            var category = await _unitOfWork.Categories.GetByIdAsync(updateProductDto.CategoryId);
            if (category == null)
                return (false, "Category does not exist.");

            _mapper.Map(updateProductDto, ormProduct);

            var product = _mapper.Map<Product>(ormProduct);
            try
            {
                product.Validate();
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message);
            }

            _unitOfWork.Products.Update(ormProduct);
            await _unitOfWork.CompleteAsync();

            return (true, "Product updated successfully.");
        }
    }
}