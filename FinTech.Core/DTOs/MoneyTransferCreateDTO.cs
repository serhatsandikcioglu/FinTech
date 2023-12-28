using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs
{
    public class MoneyTransferCreateDTO
    {
        public Guid SenderAccountId { get; set; }
        public string ReceiverAccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
