using AutoMapper;
using Domain.Ports;
using Domain.Entities;
using Application.DTO;
using Application.Interfaces;

namespace Application.UseCases.ProductsGenerics
{
    public class GetAllProductsUseCase <TModelProduct> : IGetAllProductsUseCase<TModelProduct> where TModelProduct : DomainModel
    {
        private readonly IDetailGenericRepository<TModelProduct> _repository;
        private readonly IMapper _mapper;

        public GetAllProductsUseCase(IDetailGenericRepository<TModelProduct> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product>> ExecuteAsync()
        {
           
            var products = await _repository.GetAllWithDetailAsync();
            return _mapper.Map<List<Product>>(products);
        }
    }
}