using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Entities
{
    public class MoneyTransfer : BaseEntity<Guid>
    {
        public Guid SenderAccountId { get; set; }
        public string ReceiverAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
