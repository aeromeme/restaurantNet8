using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using System.Threading.Tasks;

namespace restaurantAPIHex.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryAppService<CategoryModel> _categoryAppService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryAppService<CategoryModel> categoryAppService, IMapper mapper)
        {
            _categoryAppService = categoryAppService;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var data=  await _categoryAppService.GetAllAsync();
            var categories = _mapper.Map<IEnumerable<CategoryDto>>(data);
            return Ok(categories);
        }
    }
}
