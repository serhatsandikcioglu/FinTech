using FinTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Interfaces
{
    public interface ISupportTicketRepository
    {
        Task AddAsync(SupportTicket supportTicket);
        Task<SupportTicket> GetOldestUnprioritizedSupportRequest();
        Task<SupportTicket> GetOldestPendingPrioritySupportRequest();
        Task<SupportTicket> GetById(Guid id);
    }
}
