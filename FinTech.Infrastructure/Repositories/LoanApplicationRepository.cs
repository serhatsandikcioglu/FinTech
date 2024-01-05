using FinTech.Core.Entities;
using FinTech.Core.Enums;
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
    public class LoanApplicationRepository :GenericRepository<LoanApplication>, ILoanApplicationRepository
    {
        private readonly DbSet<LoanApplication> _dbSet;

        public LoanApplicationRepository(FinTechDbContext finTechDbContext) : base(finTechDbContext)
        {
            _dbSet = finTechDbContext.Set<LoanApplication>();
        }
        public async Task< List<PaymentStatus>>? GetPaymentStatusesByUserIdAsync(Guid applicationUserId)
        {
            var paymentStatuses = await _dbSet
            .Where(x => x.ApplicationUserId == applicationUserId)
            .SelectMany(x => x.RepaymentPlans)
            .Select(x => x.PaymentStatus)
            .ToListAsync();
            return paymentStatuses;
        }
        public async Task<List<LoanApplication>> GetAllByUserIdAsync(Guid UserId)
        {
            return await _dbSet.Where(l => l.ApplicationUserId == UserId).ToListAsync();
        }
    }
}
