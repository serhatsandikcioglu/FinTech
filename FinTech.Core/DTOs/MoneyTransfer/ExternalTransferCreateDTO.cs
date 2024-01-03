using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.MoneyTransfer
{
    public class ExternalTransferCreateDTO
    {
        public Guid SenderAccountId { get; set; }
        public string ReceiverAccountNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal Amount { get; set; }
    }
}
