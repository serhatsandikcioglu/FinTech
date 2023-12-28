using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FinTech.Shared.Constans
{
    public class MoneyTransferConstants
    {
        public const decimal DailyTransferLimit = 50000;
        public const decimal PerTransactionTransferLimit = 30000;
    }
}
