using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.DTO;
using restaurantAPI.UnitOfWork;

namespace restaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var categories = await unitOfWork.Categories.GetAllAsync();
            var categoriesDTO = mapper.Map<List<CategoryDto>>(categories);
            return Ok(categoriesDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(mapper.Map<CategoryDto>(category));
        }
    }
}
