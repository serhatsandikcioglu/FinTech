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

        public async Task AddAsync(Account account)
        {
           await _dbSet.AddAsync(account);
        }
        public async Task<string> GetBiggestAccountNumberAsync()
        {
            var biggestAccount = await _dbSet.OrderByDescending(x => x.Number).FirstOrDefaultAsync();
            return  biggestAccount?.Number;
        }
        public async Task<bool> AccountIsExistAsync(Guid accountId)
        {
            return await _dbSet.AnyAsync(x => x.Id == accountId);
        }
        public async Task< Account> GetByIdAsync(Guid accountId)
        {
            return await _dbSet.Where(x => x.Id == accountId).FirstOrDefaultAsync();
        }
        public async Task<Account> GetByAccountNumberAsync(string accountNumber)
        {
            return await _dbSet.Where(x => x.Number == accountNumber).FirstOrDefaultAsync();
        }
        public async Task<bool> AccountIsExistByAccountNumberAsync(string accountNumber)
        {
            return await _dbSet.AnyAsync(x => x.Number == accountNumber);
        }
    }
}
