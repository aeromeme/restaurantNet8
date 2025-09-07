using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.DTO;
using restaurantAPI.Application.Interfaces;
using restaurantAPI.Application.Services;

namespace restaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(ProductAppService productAppService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var products = await productAppService.getAllProducts.ExecuteAsync();
            var productDtos = mapper.Map<List<ProductDto>>(products);
            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await productAppService.getProductById.ExecuteAsync(id);
            if (product == null) return NotFound();
            return Ok(mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateProductDto dto)
        {
            var (success, message, newProductId) = await productAppService.addProduct.ExecuteAsync(dto);
            if (!success) return BadRequest(message);

            var newProduct = await productAppService.getProductById.ExecuteAsync(newProductId);
            if (newProduct == null) return BadRequest("Product creation failed.");

            var resultDto = mapper.Map<ProductDto>(newProduct);
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

            var (success, message) = await productAppService.updateProduct.ExecuteAsync(dto);
            if (!success)
            {
                if (message.Contains("does not exist", StringComparison.OrdinalIgnoreCase))
                    return NotFound(message);
                return BadRequest(message);
            }

            var updatedProduct = await productAppService.getProductById.ExecuteAsync(id);
            if (updatedProduct == null) return NotFound();

            var productDto = mapper.Map<ProductDto>(updatedProduct);
            return Ok(productDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await productAppService.deleteProduct.ExecuteAsync(id);
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
