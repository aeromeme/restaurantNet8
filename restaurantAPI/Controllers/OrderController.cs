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
    public class OrderController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var order = await unitOfWork.Orders.GetAllWithDetailAsync();
            var orderDtos = mapper.Map<List<OrderDto>>(order);
            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await unitOfWork.Orders.GetByIdWithDetailAsync(id);
            if (order == null) return NotFound();
            return Ok(mapper.Map<OrderDto>(order));
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateOrderDto dto)
        {
            var order = mapper.Map<Order>(dto);

            foreach (var detail in dto.OrderDetails)
            {
                var product = await unitOfWork.Products.GetByIdAsync(detail.ProductId);
                if (product == null)
                    return BadRequest($"Invalid product ID: {detail.ProductId}");

                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = product.Price,
                    Product = product
                });
            }

            order.TotalAmount = order.OrderDetails.Sum(d => d.UnitPrice * d.Quantity);

            await unitOfWork.Orders.AddAsync(order);
            await unitOfWork.CompleteAsync();   // persist changes

            var resultDto = mapper.Map<OrderDto>(order);
            return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, resultDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, OrderDto dto)
        {
            if (id != dto.OrderId)
                return BadRequest("Order ID mismatch.");

            var order = await unitOfWork.Orders.GetByIdWithDetailAsync(id);
            if (order == null)
                return NotFound();

            // ✅ Sync details (master–detail logic)
            // Remove deleted details
            foreach (var existing in order.OrderDetails.ToList())
            {
                if (!dto.OrderDetails.Any(d => d.OrderDetailId == existing.OrderDetailId))
                    order.OrderDetails.Remove(existing);
            }

            // Add or update details
            foreach (var detailDto in dto.OrderDetails)
            {
                var product = await unitOfWork.Products.GetByIdAsync(detailDto.ProductId);
                if (product == null)
                    return BadRequest($"Invalid product ID: {detailDto.ProductId}");

                if (detailDto.OrderDetailId == 0)
                {
                    // add new detail
                    var newDetail = mapper.Map<OrderDetail>(detailDto);
                    newDetail.Product = product; // attach product
                    order.OrderDetails.Add(newDetail);
                }
                else
                {
                    // update existing
                    var existingDetail = order.OrderDetails
                        .First(d => d.OrderDetailId == detailDto.OrderDetailId);

                    mapper.Map(detailDto, existingDetail);
                    existingDetail.Product = product; // attach product
                }
            }
            order.TotalAmount = order.OrderDetails.Sum(d => d.UnitPrice * d.Quantity);

            unitOfWork.Orders.Update(order);
            await unitOfWork.CompleteAsync();

            return NoContent(); // 204
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            unitOfWork.Orders.Remove(order);
            await unitOfWork.CompleteAsync();

            return NoContent(); // 204
        }
    }
}
