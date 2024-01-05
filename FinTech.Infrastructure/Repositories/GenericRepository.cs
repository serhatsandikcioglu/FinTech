using FinTech.Core.Interfaces;
using FinTech.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T: class
    {
        private readonly DbSet<T> _dbSet;

        public GenericRepository(FinTechDbContext finTechDbContext)
        {
            _dbSet = finTechDbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
           await _dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
           var entity = await GetByIdAsync(id);
           
            _dbSet.Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
