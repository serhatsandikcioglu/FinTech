using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.AutomaticPayment
{
    public class AutomaticPaymentCreateDTO
    {
        public Guid AccountId { get; set; }
        public string BillNumber { get; set; }
    }
}
