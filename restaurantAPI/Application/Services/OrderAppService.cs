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
            // Fetch the existing ORM entity (with details)
            var ormOrder = await _unitOfWork.Orders.GetByIdWithDetailAsync(updateOrderDto.OrderId);
            if (ormOrder == null)
                return (false, "Order does not exist.");
            var order = _mapper.Map<Order>(ormOrder);
            
            // Update main order properties
            ormOrder.CustomerName = updateOrderDto.CustomerName;
            ormOrder.OrderDate = updateOrderDto.OrderDate;
            // ... update other simple properties as needed

            // Update order details
            // Get existing details as a list for easier lookup
            var existingDetails = ormOrder.OrderDetails.ToList();

            // Build a lookup for existing details by OrderDetailId
            var existingDetailsDict = ormOrder.OrderDetails.ToDictionary(d => d.OrderDetailId);

            // Track which details are updated/added
            var processedDetailIds = new HashSet<int>();

            // 2. Update existing details using mapper
            foreach (var detailDto in updateOrderDto.OrderDetails)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(detailDto.ProductId);
                if (product == null)
                    return (false, $"Product {detailDto.ProductId} does not exist.");

                if (detailDto.OrderDetailId != 0 && existingDetailsDict.TryGetValue(detailDto.OrderDetailId, out var existing))
                {
                    // Update existing detail using mapper
                    _mapper.Map(detailDto, existing);
                    existing.UnitPrice = product.Price;
                    processedDetailIds.Add(detailDto.OrderDetailId);
                }
                else
                {
                    // Add new detail using mapper
                    var newDetail = _mapper.Map<Models.OrderDetail>(detailDto);
                    newDetail.UnitPrice = product.Price;
                    ormOrder.OrderDetails.Add(newDetail);
                }
            }

            // Delete details not present in the DTO
            var toRemove = ormOrder.OrderDetails
                .Where(d => d.OrderDetailId != 0 && !updateOrderDto.OrderDetails.Any(dto => dto.OrderDetailId == d.OrderDetailId))
                .ToList();

            foreach (var detail in toRemove)
            {
                ormOrder.OrderDetails.Remove(detail);
            }

            // Recalculate total amount
            ormOrder.TotalAmount = ormOrder.OrderDetails.Sum(d => d.UnitPrice * d.Quantity);

            order = _mapper.Map<Order>(ormOrder);
            try
            {
                order.Validate(); // Domain logic
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message);
            }

            // Optionally validate domain logic here

            _unitOfWork.Orders.Update(ormOrder); // or rely on tracking
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