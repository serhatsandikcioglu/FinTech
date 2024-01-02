using FinTech.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Entities
{
    public class RepaymentPlan : BaseEntity<Guid>
    {
        public Guid LoanApplicationId { get; set; }
        public LoanApplication LoanApplication { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
