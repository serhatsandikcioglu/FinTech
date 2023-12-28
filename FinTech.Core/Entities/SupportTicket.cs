using FinTech.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Entities
{
    public class SupportTicket : BaseEntity<Guid>
    {
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public string TicketDescription { get; set; }
    }
}
