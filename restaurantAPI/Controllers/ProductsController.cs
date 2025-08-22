using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurantAPI.DTO;
using restaurantAPI.Models;
using restaurantAPI.UnitOfWork;

namespace restaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var products = await unitOfWork.Products.GetAllWithCategoryAsync();
            var productDtos = mapper.Map<List<ProductDto>>(products);
            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await unitOfWork.Products.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();
            return Ok(mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateProductDto dto)
        {
            var product = mapper.Map<Product>(dto);
            // Optional: attach existing category if using foreign key
            if ( product.CategoryId != 0)
            {
                // Load category from DB to avoid inserting a new one
                var existingCategory = await unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
                if (existingCategory == null) return BadRequest("Invalid category ID.");
                product.Category = existingCategory;
            }
            product.StockQuantity = 0; // Initialize stock to 0
            await unitOfWork.Products.AddAsync(product);
            await unitOfWork.CompleteAsync();   // persist changes

            var resultDto = mapper.Map<ProductDto>(product);
            return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, resultDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, UpdateProductDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Product ID mismatch.");

            var product = await unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            if (product.CategoryId != 0)
            {
                // Load category from DB to avoid inserting a new one
                var existingCategory = await unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
                if (existingCategory == null) return BadRequest("Invalid category ID.");
                product.Category = existingCategory;
            }
            // Map DTO → Entity
            mapper.Map(dto, product);

            unitOfWork.Products.Update(product);
            await unitOfWork.CompleteAsync();

            return NoContent(); // 204
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            unitOfWork.Products.Remove(product);
            await unitOfWork.CompleteAsync();

            return NoContent(); // 204
        }

    }
}
