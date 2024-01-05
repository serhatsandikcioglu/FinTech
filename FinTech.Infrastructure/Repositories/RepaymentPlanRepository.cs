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
    public class RepaymentPlanRepository : GenericRepository<RepaymentPlan>,IRepaymentPlanRepository
    {
        private readonly DbSet<RepaymentPlan> _dbSet;

        public RepaymentPlanRepository(FinTechDbContext finTechDbContext) : base(finTechDbContext)
        {
            _dbSet = finTechDbContext.Set<RepaymentPlan>();
        }

        public async Task AddAsync(RepaymentPlan repaymentPlan)
        {
          await  _dbSet.AddAsync(repaymentPlan);
        }
    }
}
