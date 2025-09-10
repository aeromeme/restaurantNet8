using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Application.UseCases.OrdersGenerics;
using Application.Interfaces;

namespace restaurantAPIHex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGetAllOrderUseCase<OrderModel> _getAllOrderUseCase;    
        private readonly IMapper _mapper;
        public OrderController(IGetAllOrderUseCase<OrderModel> getAllOrderUseCase, IMapper mapper)
        {
            _getAllOrderUseCase = getAllOrderUseCase;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data= await _getAllOrderUseCase.ExecuteAsync();
            var orders= _mapper.Map<List<Application.DTO.OrderDto>>(data);
            return Ok(orders);
        }
    }
}
