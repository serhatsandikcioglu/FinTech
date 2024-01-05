using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdentityNumber { get; set; }
        public string Address { get; set; }
        public List<Account> Accounts { get; set; }
        public List<LoanApplication> LoanApplications { get; set; }
        public List<SupportTicket> SupportTickets { get; set; }
    }
}
