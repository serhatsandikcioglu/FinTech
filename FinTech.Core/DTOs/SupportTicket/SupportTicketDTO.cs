using FinTech.Core.Entities;
using FinTech.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.SupportTicket
{
    public class SupportTicketDTO
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public TicketPriorityLevel? PriorityLevel { get; set; }
        public TicketStatus Status { get; set; }
        public string Description { get; set; }
    }
}
