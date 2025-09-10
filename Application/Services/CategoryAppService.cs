using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryAppService <TCategoryModel> : ICategoryAppService<TCategoryModel> where TCategoryModel : DomainModel
    {
        private readonly IRepository<TCategoryModel> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryAppService(IUnitOfWorkGeneric unitOfWork, IMapper mapper, IRepository<TCategoryModel> categoryRepository )
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync() {
           
            var data=await _categoryRepository.GetAllAsync();
            var categories = _mapper.Map<List<Category>>(data);
            return categories;

        } 
    }
}
