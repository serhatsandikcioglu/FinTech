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
        void Add(MoneyTransfer moneyTransfer);
        List<decimal> GetDailyTransferAmount(Guid senderAccountId, DateTime date);
    }
}
