﻿using FinTech.Core.Entities;
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
    public class AccountActivityRepository : GenericRepository<AccountActivity>, IAccountActivityRepository
    {
        private readonly DbSet<AccountActivity> _dbSet;

        public AccountActivityRepository(FinTechDbContext finTechDbContext) : base(finTechDbContext)
        {
            _dbSet = finTechDbContext.Set<AccountActivity>();
        }
        public async Task<List<AccountActivity>> GetAllByAccountIdAsync(Guid accountId)
        {
            return await _dbSet.Where(x=>x.AccountId == accountId).ToListAsync();
        }
    }
}
