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

        public async Task AddAsync(SupportTicket supportTicket)
        {
            await _dbSet.AddAsync(supportTicket);
        }
        public async Task<SupportTicket> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<SupportTicket> GetOldestUnprioritizedSupportRequest()
        {
            var ticket = _dbSet
                .Where(t => t.PriorityLevel == null)
                .OrderBy(t => t.CreatedDate)
                .FirstOrDefault();

            return ticket;
        }
        public async Task<SupportTicket> GetOldestPendingPrioritySupportRequest()
        {
            var ticket = _dbSet
                .Where(ticket => ticket.Status == TicketStatus.Pending && ticket.PriorityLevel.HasValue)
                .OrderByDescending(ticket => ticket.PriorityLevel)
                .ThenBy(ticket => ticket.CreatedDate)
                .FirstOrDefault();

            return ticket;
        }
    }
}
