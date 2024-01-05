using FinTech.Core.Entities;
using FinTech.Core.Interfaces;
using FinTech.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Infrastructure.Repositories
{
    public class BillRepository :GenericRepository<Bill>, IBillRepository
    {
        private readonly DbSet<Bill> _dbSet;

        public BillRepository(FinTechDbContext finTechDbContext) : base(finTechDbContext)
        {
            _dbSet = finTechDbContext.Set<Bill>();
        }
        public async Task<List<Bill>> GetByNumberAsync(string billNumber)
        {
            return await _dbSet.Where(b => b.Number == billNumber && !b.IsPaid && b.DueDate.Date <= DateTime.UtcNow.Date).ToListAsync();
        }
    }
}
