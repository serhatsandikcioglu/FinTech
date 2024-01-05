using FinTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Interfaces
{
    public interface ISupportTicketRepository : IGenericRepository<SupportTicket>
    {
        Task<SupportTicket> GetOldestUnprioritizedSupportRequestAsync();
        Task<SupportTicket> GetOldestPendingPrioritySupportRequestAsync();
    }
}
