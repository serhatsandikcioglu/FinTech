using FinTech.Core.Entities;
using FinTech.Core.Interfaces;
using FinTech.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DbSet<Account> _dbSet;

        public AccountRepository(FinTechDbContext finTechDbContext)
        {
            _dbSet = finTechDbContext.Set<Account>();
        }

        public void Add(Account account)
        {
            _dbSet.Add(account);
        }
        public string GetBiggestAccountNumber()
        {
            var biggestAccount = _dbSet.OrderByDescending(x => x.Number).FirstOrDefault();
            return biggestAccount?.Number;
        }
        public decimal GetBalanceByAccountId(Guid accountId)
        {
            return _dbSet.Where(x=>x.Id == accountId).Select(x => x.Balance).FirstOrDefault();
        }
        public bool AccountIsExist(Guid accountId)
        {
            return _dbSet.Any(x => x.Id == accountId);
        }
        public Account GetById(Guid accountId)
        {
            return _dbSet.Where(x => x.Id == accountId).FirstOrDefault();
        }
        public Account GetByAccountNumber(string accountNumber)
        {
            return _dbSet.Where(x => x.Number == accountNumber).FirstOrDefault();
        }
    }
}
