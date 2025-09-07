using restaurantAPI.UnitOfWork;

public class DeleteProductUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(bool Success, string Message)> ExecuteAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
        {
            return (false, "Product does not exist.");
        }

        _unitOfWork.Products.Remove(product);
        await _unitOfWork.CompleteAsync();

        return (true, "Product deleted successfully.");
    }
}