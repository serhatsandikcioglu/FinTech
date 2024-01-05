using FinTech.Core.DTOs.SupportTicket;
using FinTech.Core.Enums;
using FinTech.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Interfaces
{
    public interface ISupportTicketService
    {
        Task<CustomResponse<SupportTicketCreatedDTO>> CreateAsync(Guid userId, SupportTicketCreateDTO supportTicketCreateDTO);
        Task<CustomResponse<SupportTicketDTO>> GetByWithoutPrioritizationStatusAsync();
        Task<CustomResponse<SupportTicketDTO>> GetByPriorityStatusAsync();
        Task<CustomResponse<NoContent>> ProcessAsync(Guid supportTicketId);
        Task<CustomResponse<NoContent>> DeterminePriortyLevelAsync(Guid supportTicketId, TicketPriorityLevel ticketPriorityLevel);
    }
}
