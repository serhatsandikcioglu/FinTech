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
    public class MoneyTransferRepository : GenericRepository<MoneyTransfer>,IMoneyTransferRepository
    {
        private readonly DbSet<MoneyTransfer> _dbSet;

        public MoneyTransferRepository(FinTechDbContext finTechDbContext) : base(finTechDbContext)
        {
            _dbSet = finTechDbContext.Set<MoneyTransfer>();
        }
        public async Task<List<decimal>> GetDailyTransferAmountAsync(Guid senderAccountId , DateTime date)
        {
            List<decimal> amounts = await _dbSet.Where(x => x.SenderAccountId == senderAccountId && x.Date.Date == date.Date).Select(x=>x.Amount).ToListAsync();
            return amounts;
        }
    }
}
