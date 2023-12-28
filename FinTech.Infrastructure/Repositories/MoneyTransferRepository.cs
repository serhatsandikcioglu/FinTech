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
    public class MoneyTransferRepository : IMoneyTransferRepository
    {
        private readonly DbSet<MoneyTransfer> _dbSet;

        public MoneyTransferRepository(FinTechDbContext finTechDbContext)
        {
            _dbSet = finTechDbContext.Set<MoneyTransfer>();
        }

        public void Add(MoneyTransfer moneyTransfer)
        {
            _dbSet.Add(moneyTransfer);
        }
        public List<decimal> GetDailyTransferAmount(Guid senderAccountId , DateTime date)
        {
            List<decimal> amounts = _dbSet.Where(x => x.SenderAccountId == senderAccountId && x.Date.Date == date.Date).Select(x=>x.Amount).ToList();
            return amounts;
        }
    }
}
