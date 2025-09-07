using AutoMapper;
using restaurantAPI.UnitOfWork;
using restaurantAPI.Domain.Entities;

public class GetProductByIdUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductEntity?> ExecuteAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);
        return product == null ? null : _mapper.Map<ProductEntity>(product);
    }
}