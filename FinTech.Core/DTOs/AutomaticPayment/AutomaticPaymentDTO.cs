using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.AutomaticPayment
{
    public class AutomaticPaymentDTO
    {
        public Guid Id { get; set; }
        public string BillNumber { get; set; }
    }
}
