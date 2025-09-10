using AutoMapper;
using Domain.Ports;
using Domain.Entities;
using Application.DTO;
using Application.Interfaces;

namespace Application.UseCases.ProductsGenerics
{
    public class GetProductByIdUseCase <TModelProduct> : IGetProductByIdUseCase<TModelProduct> where TModelProduct : DomainModel
    {
        private readonly IDetailGenericRepository<TModelProduct> _repository;
        private readonly IMapper _mapper;

        public GetProductByIdUseCase(IDetailGenericRepository<TModelProduct> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Product?> ExecuteAsync(int id)
        {
            var product = await _repository.GetByIdWithDetailAsync(id);
            return product == null ? null : _mapper.Map<Product>(product);
        }
    }
}