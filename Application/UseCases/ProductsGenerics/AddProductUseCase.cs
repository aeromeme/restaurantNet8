using AutoMapper;
using Domain.Ports;
using Domain.Entities;
using Application.DTO;
using Application.Interfaces;

namespace Application.UseCases.ProductsGenerics
{
    public class AddProductUseCase<TCategoryModel, TProductModel> : IAddProductUseCase<TCategoryModel, TProductModel>
        where TCategoryModel : DomainModel
        where TProductModel : DomainModel
    {
        private readonly IUnitOfWorkGeneric _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDetailGenericRepository<TProductModel> _productRepository;
        private readonly IRepository<TCategoryModel> _categoryRepository;

        public AddProductUseCase(IUnitOfWorkGeneric unitOfWork, IMapper mapper, IDetailGenericRepository<TProductModel> productRepository, IRepository<TCategoryModel> categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<(bool Success, string Message, int id)> ExecuteAsync(CreateProductDto createProductDto)
        {
            var category = await _categoryRepository.GetByIdAsync(createProductDto.CategoryId);
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

            var ormProduct = _mapper.Map<TProductModel>(product);

            await _productRepository.AddAsync(ormProduct);
            await _unitOfWork.CompleteAsync();

            //var idProperty = typeof(TProductModel).GetProperty("ProductId") ?? typeof(TProductModel).GetProperty("Id");
            //var id= (int)(idProperty?.GetValue(ormProduct) ?? 0);
            var id = ormProduct.GetId(); // Assuming ProductId is the identifier property

            return (true, "Product created successfully.", id);
        }
    }

}
