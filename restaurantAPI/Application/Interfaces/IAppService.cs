namespace restaurantAPI.Application.Interfaces
{
    public interface IAppService<T,TCreateDTO,TUpdateDTO>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<(bool Success, string Message, int? NewId)> AddAsync(TCreateDTO item);
        Task<(bool Success, string Message)> UpdateAsync(TUpdateDTO item);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
