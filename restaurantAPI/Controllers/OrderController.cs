using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.Application.Interfaces;
using restaurantAPI.DTO;

namespace restaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderAppService orderAppService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var orders = await orderAppService.GetAllAsync();
            var orderDtos = mapper.Map<List<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await orderAppService.GetByIdAsync(id);
            if (order == null) return NotFound();
            var orderDto = mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateOrderDto dto)
        {
            var result = await orderAppService.AddAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);

            // Optionally, fetch the created order to return
            var createdOrder = await orderAppService.GetByIdAsync(result.NewOrderId ?? 0);
            if (createdOrder == null) return BadRequest("Product creation failed.");

            var resultDto = mapper.Map<OrderDto>(createdOrder);
            return CreatedAtAction(nameof(GetById), new { id = result.NewOrderId }, resultDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, OrderDto dto)
        {
            if (id != dto.OrderId)
                return BadRequest("Order ID mismatch.");

            var result = await orderAppService.UpdateAsync(dto);
            if (!result.Success)
            {
                if (result.Message.Contains("does not exist"))
                    return NotFound(result.Message);
                return BadRequest(result.Message);
            }

            var updatedOrder = await orderAppService.GetByIdAsync(id);
            if (updatedOrder == null) return NotFound();

            var orderDto = mapper.Map<OrderDto>(updatedOrder);
            return Ok(orderDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await orderAppService.DeleteAsync(id);
            if (!result.Success)
            {
                if (result.Message.Contains("does not exist"))
                    return NotFound(result.Message);
                return BadRequest(result.Message);
            }
            return NoContent();
        }
    }
}
