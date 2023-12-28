using FinTech.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Entities
{
    public class AccountActivity : BaseEntity<Guid>
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime Date { get; set; }
    }
}
