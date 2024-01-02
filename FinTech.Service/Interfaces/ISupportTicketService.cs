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
        Task<CustomResponse<SupportTicketCreatedDTO>> Create(Guid userId, SupportTicketCreateDTO supportTicketCreateDTO);
        Task<CustomResponse<SupportTicketDTO>> GetByWithoutPrioritizationStatus();
        Task<CustomResponse<SupportTicketDTO>> GetByPriorityStatus();
    }
}
