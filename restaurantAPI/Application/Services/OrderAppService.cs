using AutoMapper;
using restaurantAPI.Application.Interfaces;
using restaurantAPI.Domain.Entities;
using restaurantAPI.DTO;
using restaurantAPI.UnitOfWork;

namespace restaurantAPI.Application.Services
{
    public class OrderAppService : IOrderAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderAppService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllWithDetailAsync();
            return _mapper.Map<List<Order>>(orders);
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdWithDetailAsync(id);
            return order == null ? null : _mapper.Map<Order>(order);
        }

        public async Task<(bool Success, string Message, int? NewOrderId)> AddAsync(CreateOrderDto createOrderDto)
        {
            var order = _mapper.Map<Order>(createOrderDto);

            // Validate and map order details
            foreach (var detail in order.OrderDetails)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId ?? 0);
                if (product == null)
                    return (false, $"Product {detail.ProductId} does not exist.", null);

                detail.UnitPrice = product.Price;
                detail.Validate();
            }

            order.OrderDate = DateOnly.FromDateTime(DateTime.Now);
            order.TotalAmount = order.OrderDetails.Sum(d => d.UnitPrice * d.Quantity);

            try
            {
                order.Validate();
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message, null);
            }

            var ormOrder = _mapper.Map<Models.Order>(order);
            await _unitOfWork.Orders.AddAsync(ormOrder);
            await _unitOfWork.CompleteAsync();

            return (true, "Order created successfully.", ormOrder.OrderId);
        }

        public async Task<(bool Success, string Message)> UpdateAsync(OrderDto updateOrderDto)
        {
            var existingOrder = await _unitOfWork.Orders.GetByIdWithDetailAsync(updateOrderDto.OrderId);
            if (existingOrder == null)
                return (false, "Order does not exist.");

            // Map updates
            var order = _mapper.Map<Order>(updateOrderDto);

            // Validate and map order details
            foreach (var detail in order.OrderDetails)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId ?? 0);
                if (product == null)
                    return (false, $"Product {detail.ProductId} does not exist.");

                detail.UnitPrice = product.Price;
                detail.Validate();
            }

            order.TotalAmount = order.OrderDetails.Sum(d => d.UnitPrice * d.Quantity);

            try
            {
                order.Validate();
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message);
            }

            var ormOrder = _mapper.Map<Models.Order>(order);
            _unitOfWork.Orders.Update(ormOrder);
            await _unitOfWork.CompleteAsync();

            return (true, "Order updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var ormOrder = await _unitOfWork.Orders.GetByIdWithDetailAsync(id);
            if (ormOrder == null)
                return (false, "Order does not exist.");

            // Map ORM model to domain entity
            var domainOrder = _mapper.Map<Order>(ormOrder);

            // Apply domain logic (simulate a call)
            try
            {
                domainOrder.Validate();
                // You can add more domain logic here if needed
            }
            catch (ArgumentException ex)
            {
                return (false, $"Domain validation failed: {ex.Message}");
            }

            // Proceed with removal
            _unitOfWork.Orders.Remove(ormOrder);
            await _unitOfWork.CompleteAsync();

            return (true, "Order deleted successfully.");
        }
    }
}