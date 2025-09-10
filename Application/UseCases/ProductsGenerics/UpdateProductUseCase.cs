using AutoMapper;
using Domain.Ports;
using Domain.Entities;
using Application.DTO;
using Application.Interfaces;

namespace Application.UseCases.ProductsGenerics
{
    public class UpdateProductUseCase<TCategoryModel,TModelProduct> : IUpdateProductUseCase<TCategoryModel, TModelProduct>
                where TCategoryModel : DomainModel
        where TModelProduct : DomainModel
    {
        private readonly IUnitOfWorkGeneric _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDetailGenericRepository<TModelProduct> _productRepository;
        private readonly IRepository<TCategoryModel> _categoryRepository;

        public UpdateProductUseCase(IUnitOfWorkGeneric unitOfWork, IMapper mapper, IDetailGenericRepository<TModelProduct> productRepository, IRepository<TCategoryModel> categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(UpdateProductDto updateProductDto)
        {
         
            var ormProduct = await _productRepository.GetByIdAsync(updateProductDto.Id);
            if (ormProduct == null)
                return (false, "Product does not exist.");

            var category = await _categoryRepository.GetByIdAsync(updateProductDto.CategoryId);
            if (category == null)
                return (false, "Category does not exist.");

            

            var product = _mapper.Map<Product>(ormProduct);
            //bussiness logic too
            _mapper.Map(updateProductDto, product);

            try
            {
                product.Validate();
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message);
            }

            _mapper.Map(product, ormProduct);

            _productRepository.Update(ormProduct);
            await _unitOfWork.CompleteAsync();

            return (true, "Product updated successfully.");
        }
    }
}