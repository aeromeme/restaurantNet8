using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWork
{
    public class UnitOfWorkGeneric : IUnitOfWorkGeneric
    {
        private readonly RestaurantContext _context;

        public UnitOfWorkGeneric(RestaurantContext context)
        {
            _context = context;
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
