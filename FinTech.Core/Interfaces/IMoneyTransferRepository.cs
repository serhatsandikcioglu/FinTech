using FinTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Interfaces
{
    public interface IMoneyTransferRepository
    {
        Task AddAsync(MoneyTransfer moneyTransfer);
        Task<List<decimal>> GetDailyTransferAmountAsync(Guid senderAccountId, DateTime date);
    }
}
