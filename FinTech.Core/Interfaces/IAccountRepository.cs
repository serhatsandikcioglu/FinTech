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
    public interface IAccountRepository
    {
        void Add(Account account);
        public string GetBiggestAccountNumber();
        decimal GetBalanceByAccountId(Guid accountId);
        bool AccountIsExist(Guid accountId);
        Account GetById(Guid accountId);
        Account GetByAccountNumber(string accountNumber);
    }
}
