using FinTech.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Entities
{
    public class LoanApplication : BaseEntity<Guid>
    {
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public decimal Amount { get; set; }
        public int MaturityTerm { get; set; }
        public string CreditScoreResultComment { get; set; }
        public LoanApllicationStatus Status { get; set; }
        public DateTime Date { get; set; }
        public List<RepaymentPlan>? RepaymentPlans { get; set; }
    }
}
