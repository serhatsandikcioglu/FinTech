using FinTech.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task AddAsync(Account account);
        Task<string> GetBiggestAccountNumberAsync();
        Task<bool> AccountIsExistAsync(Guid accountId);
        Task<Account> GetByIdAsync(Guid accountId);
        Task<Account> GetByAccountNumberAsync(string accountNumber);
        Task<bool> AccountIsExistByAccountNumberAsync(string accountNumber);
    }
}
