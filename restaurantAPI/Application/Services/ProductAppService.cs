using AutoMapper;
using restaurantAPI.Application.Interfaces;
using restaurantAPI.Domain.Entities;
using restaurantAPI.UnitOfWork;
using restaurantAPI.DTO; // Assuming CreateProductDto is here

namespace restaurantAPI.Application.Services
{
    public class ProductAppService : IProductAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductAppService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllWithCategoryAsync();
            return _mapper.Map<List<Product>>(products);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);
            return product == null ? null : _mapper.Map<Product>(product);
        }

        public async Task<(bool Success, string Message, int id)> AddAsync(CreateProductDto createProductDto)
        {
            // Check if category exists
            var category = await _unitOfWork.Categories.GetByIdAsync(createProductDto.CategoryId);
            if (category == null)
            {
                return (false, "Category does not exist.",0);
            }

            // Map DTO to domain entity
            var product = _mapper.Map<Product>(createProductDto);

            try
            {
                product.Validate(); // Domain logic
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message,0);
            }

            // Map domain entity to ORM model if needed
            var ormProduct = _mapper.Map<Models.Product>(product);

            await _unitOfWork.Products.AddAsync(ormProduct);
            await _unitOfWork.CompleteAsync();

            return (true, "Product created successfully.", ormProduct.ProductId);
        }

        public async Task<(bool Success, string Message)> UpdateAsync(UpdateProductDto updateProductDto)
        {
            // Fetch the existing ORM entity
            var ormProduct = await _unitOfWork.Products.GetByIdAsync(updateProductDto.Id);
            if (ormProduct == null)
                return (false, "Product does not exist.");

            // Check if category exists
            var category = await _unitOfWork.Categories.GetByIdAsync(updateProductDto.CategoryId);
            if (category == null)
                return (false, "Category does not exist.");

            // Update properties using mapper
            _mapper.Map(updateProductDto, ormProduct);

            // Optionally validate domain logic here
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

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return (false, "Product does not exist.");
            }

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CompleteAsync();

            return (true, "Product deleted successfully.");
        }
    }
}