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
    public class AutomaticPaymentRepository :GenericRepository<AutomaticPayment>, IAutomaticPaymentRepository
    {
        private readonly DbSet<AutomaticPayment> _dbSet;

        public AutomaticPaymentRepository(FinTechDbContext finTechDbContext) : base(finTechDbContext)
        {
            _dbSet = finTechDbContext.Set<AutomaticPayment>();
        }

        public async Task AddAsync(AutomaticPayment automaticPayment)
        {
           await _dbSet.AddAsync(automaticPayment);
        }
        public async Task<List<AutomaticPayment>> GetAllByUserId(Guid userId)
        {
            return await _dbSet.Where(a=>a.userId == userId).ToListAsync();
        }
        public async Task<bool> IsExistAsync(Guid automaticPaymentId)
        {
            return await _dbSet.AnyAsync(a =>a.Id == automaticPaymentId);
        }
    }
}
