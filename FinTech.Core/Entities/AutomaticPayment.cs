using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Entities
{
    public class AutomaticPayment : BaseEntity<Guid>
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public string BillNumber { get; set; }
        public Guid userId { get; set; }
    }
}
