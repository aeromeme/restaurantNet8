using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public interface IDetailGenericRepository<TModel> : IRepository<TModel>
            where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllWithDetailAsync();
        Task<TModel?> GetByIdWithDetailAsync(int id);
    }
}
