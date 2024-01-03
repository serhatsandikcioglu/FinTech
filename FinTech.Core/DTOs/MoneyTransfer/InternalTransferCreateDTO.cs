using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.MoneyTransfer
{
    public class InternalTransferCreateDTO
    {
        public Guid SenderAccountId { get; set; }
        public Guid ReceiverAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
