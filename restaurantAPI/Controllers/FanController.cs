using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.Application.Interfaces;
using restaurantAPI.Domain.Entities;
using restaurantAPI.DTO;

namespace restaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FanController : ControllerBase
    {
        private readonly IAppService<restaurantAPI.Domain.Entities.Fan, CreateFanDto, restaurantAPI.Domain.Entities.Fan> _fanAppService;

        public FanController(IAppService<restaurantAPI.Domain.Entities.Fan, CreateFanDto, restaurantAPI.Domain.Entities.Fan> fanAppService)
        {
            _fanAppService = fanAppService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Fan>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var fans = await _fanAppService.GetAllAsync();
            return Ok(fans);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Fan), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var fan = await _fanAppService.GetByIdAsync(id);
            if (fan == null)
                return NotFound();
            return Ok(fan);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateFanDto createFanDto)
        {
            var (Success, Message, NewId) = await _fanAppService.AddAsync(createFanDto);
            if (!Success)
                return BadRequest(Message);
            return CreatedAtAction(nameof(GetById), new { id = NewId }, NewId);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var (Success, Message) = await _fanAppService.DeleteAsync(id);
            if (!Success)
                return NotFound(Message);
            return Ok(Message);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Fan updateFanDto)
        {
            var (Success, Message) = await _fanAppService.UpdateAsync(updateFanDto);
            if (!Success)
                return BadRequest(Message);
            return Ok(Message);
        }
    }
}
