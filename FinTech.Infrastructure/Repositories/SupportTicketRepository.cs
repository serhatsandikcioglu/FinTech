using FinTech.Core.Entities;
using FinTech.Core.Enums;
using FinTech.Core.Interfaces;
using FinTech.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Infrastructure.Repositories
{
    public class SupportTicketRepository : GenericRepository<SupportTicket>,ISupportTicketRepository
    {
        private readonly DbSet<SupportTicket> _dbSet;

        public SupportTicketRepository(FinTechDbContext finTechDbContext) : base(finTechDbContext)
        {
            _dbSet = finTechDbContext.Set<SupportTicket>();
        }

        public async Task<SupportTicket> GetOldestUnprioritizedSupportRequestAsync()
        {
            var ticket = _dbSet
                .Where(t => t.PriorityLevel == null)
                .OrderBy(t => t.CreatedDate)
                .FirstOrDefault();

            return ticket;
        }
        public async Task<SupportTicket> GetOldestPendingPrioritySupportRequestAsync()
        {
            var ticket = _dbSet
                .Where(ticket => ticket.Status == TicketStatus.Pending && ticket.PriorityLevel.HasValue)
                .OrderByDescending(ticket => ticket.PriorityLevel)
                .ThenBy(ticket => ticket.CreatedDate)
                .FirstOrDefault();

            return ticket;
        }
        public async Task<List<SupportTicket>> GetAllByUserIdAsync(Guid UserId)
        {
            return await _dbSet.Where(s => s.ApplicationUserId == UserId).ToListAsync();
        }
    }
}
