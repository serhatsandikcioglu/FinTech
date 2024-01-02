using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Core.Entities
{
    public class Account : BaseEntity <Guid>
    {
        public string Number { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<AccountActivity> AccountActivities { get; set; }
        public List<AutomaticPayment> AutomaticPayments { get; set; }
    }
}
