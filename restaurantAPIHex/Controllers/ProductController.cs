using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;

namespace restaurantAPIHex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IGetAllProductsUseCase<ProductModel> _getAllProductsUseCase;
        private readonly IGetProductByIdUseCase<ProductModel> _getProductByIdUseCase;
        private readonly IAddProductUseCase<CategoryModel, ProductModel> _productAppService;
        private readonly IUpdateProductUseCase<CategoryModel, ProductModel> _updateProductUseCase;
        private readonly IDeleteProductUseCase<ProductModel> _deleteProductUseCase;
        private readonly AutoMapper.IMapper _mapper;
        public ProductController(IGetAllProductsUseCase<ProductModel> getAllProductsUseCase, IMapper _map, IGetProductByIdUseCase<ProductModel> getProductByIdUseCase, IAddProductUseCase<CategoryModel, ProductModel> productAppService,IUpdateProductUseCase<CategoryModel,ProductModel> updateProductUseCase ,  IDeleteProductUseCase<ProductModel> deleteProductUseCase)
        {
            _getAllProductsUseCase = getAllProductsUseCase;
            _getProductByIdUseCase = getProductByIdUseCase;
            _mapper = _map;
            _productAppService = productAppService;
            _updateProductUseCase = updateProductUseCase;
            _deleteProductUseCase = deleteProductUseCase;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var products = await _getAllProductsUseCase.ExecuteAsync();
             var data =_mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(products);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _getProductByIdUseCase.ExecuteAsync(id);
            if (product == null) return NotFound();
            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateProductDto dto)
        {
            var (success, message, newProductId) = await _productAppService.ExecuteAsync(dto);
            if (!success) return BadRequest(message);

            var newProduct = await _getProductByIdUseCase.ExecuteAsync(newProductId);
            if (newProduct == null) return BadRequest("Product creation failed.");

            var resultDto = _mapper.Map<ProductDto>(newProduct);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.ProductId }, resultDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, UpdateProductDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Product ID mismatch.");

            var (success, message) = await _updateProductUseCase.ExecuteAsync(dto);
            if (!success)
            {
                if (message.Contains("does not exist", StringComparison.OrdinalIgnoreCase))
                    return NotFound(message);
                return BadRequest(message);
            }

            var updatedProduct = await _getProductByIdUseCase.ExecuteAsync(id);
            if (updatedProduct == null) return NotFound();

            var productDto = _mapper.Map<ProductDto>(updatedProduct);
            return Ok(productDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _deleteProductUseCase.ExecuteAsync(id);
            if (!success)
            {
                if (message.Contains("does not exist", StringComparison.OrdinalIgnoreCase))
                    return NotFound(message);
                return BadRequest(message);
            }
            return NoContent(); // 204
        }
    }
}
