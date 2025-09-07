using restaurantAPI.Application.Interfaces;
using restaurantAPI.Application.Products.UseCases;

namespace restaurantAPI.Application.Services
{
    public class ProductAppService 
    {
        public readonly GetAllProductsUseCase getAllProducts;
        public readonly GetProductByIdUseCase getProductById;
        public readonly AddProductUseCase addProduct;
        public readonly UpdateProductUseCase updateProduct;
        public readonly DeleteProductUseCase deleteProduct;

        public ProductAppService(
            GetAllProductsUseCase getAllProducts,
            GetProductByIdUseCase getProductById,
            AddProductUseCase addProduct,
            UpdateProductUseCase updateProduct,
            DeleteProductUseCase deleteProduct)
        {
            this.getAllProducts = getAllProducts;
            this.getProductById = getProductById;
            this.addProduct = addProduct;
            this.updateProduct = updateProduct;
            this.deleteProduct = deleteProduct;
        }
    }
}