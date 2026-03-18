using AutoMapper;
using restaurantAPI.Application.Interfaces;
using restaurantAPI.Domain.Entities;
using restaurantAPI.DTO;
using restaurantAPI.UnitOfWork;

namespace restaurantAPI.Application.Services
{
    public class FanAppService : IAppService<Fan, CreateFanDto, Fan>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FanAppService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<(bool Success, string Message, int? NewId)> AddAsync(CreateFanDto item)
        {
            var fan = _mapper.Map<Fan>(item);

            try
            {
                fan.Validate();
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message, 0);
            }

            var ormFan = _mapper.Map<restaurantAPI.Models.Fan>(fan);

            await _unitOfWork.Fans.AddAsync(ormFan);
            await _unitOfWork.CompleteAsync();

            return (true, "Fan created successfully.", ormFan.Id);
        }

        public Task<(bool Success, string Message)> DeleteAsync(int id)
        {
           var ormFan = _unitOfWork.Fans.GetByIdAsync(id).Result;
            if (ormFan == null)
            {
                return Task.FromResult((false, "Fan not found."));
            }
            _unitOfWork.Fans.Remove(ormFan);
            _unitOfWork.CompleteAsync().Wait();
            return Task.FromResult((true, "Fan deleted successfully."));
        }

        public Task<IEnumerable<Fan>> GetAllAsync()
        {
           var ormFans = _unitOfWork.Fans.GetAllAsync().Result;
            var fans = _mapper.Map<List<Fan>>(ormFans);
            return Task.FromResult<IEnumerable<Fan>>(fans);
        }

        public Task<Fan?> GetByIdAsync(int id)
        {
           var ormFan = _unitOfWork.Fans.GetByIdAsync(id).Result;
            if (ormFan == null)
            {
                return Task.FromResult<Fan?>(null);
            }
            var fan = _mapper.Map<Fan>(ormFan);
            return Task.FromResult<Fan?>(fan);
        }

        public Task<(bool Success, string Message)> UpdateAsync(Fan item)
        {
           var ormFan = _unitOfWork.Fans.GetByIdAsync(item.Id).Result;
            if (ormFan == null)
            {
                return Task.FromResult((false, "Fan not found."));
            }
            var updatedFan = _mapper.Map(item, ormFan);
            var fan = _mapper.Map<Fan>(updatedFan);
            try
            {
                fan.Validate();
            }
            catch (ArgumentException ex)
            {
                return Task.FromResult((false, ex.Message));
            }
            _unitOfWork.CompleteAsync().Wait();
            return Task.FromResult((true, "Fan updated successfully."));
        }
    }
}
