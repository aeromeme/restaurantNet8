using Domain.Entities;
using Domain.Ports;

namespace Application.Interfaces
{
    public interface ICategoryAppService<TCategoryModel>
        where TCategoryModel : DomainModel
    {
        Task<IEnumerable<Category>> GetAllAsync();
    }
}