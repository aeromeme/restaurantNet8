using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.OrdersGenerics
{
    public class GetAllOrderUseCase<TOrderModel> : IGetAllOrderUseCase<TOrderModel> where TOrderModel : DomainModel
    {
        private readonly IDetailGenericRepository<TOrderModel> _repository;
        private readonly IMapper _mapper;

        public GetAllOrderUseCase(IDetailGenericRepository<TOrderModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Order>> ExecuteAsync()
        {

            var orders = await _repository.GetAllWithDetailAsync();
            return _mapper.Map<List<Order>>(orders);
        }

    }
}
