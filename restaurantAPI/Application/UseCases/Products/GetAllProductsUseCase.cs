using AutoMapper;
using restaurantAPI.UnitOfWork;
using restaurantAPI.Domain.Entities;

public class GetAllProductsUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllProductsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductEntity>> ExecuteAsync()
    {
        var products = await _unitOfWork.Products.GetAllWithCategoryAsync();
        return _mapper.Map<List<ProductEntity>>(products);
    }
}