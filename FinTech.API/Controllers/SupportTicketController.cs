using FinTech.Core.Constans;
using FinTech.Core.DTOs.SupportTicket;
using FinTech.Core.DTOs.User;
using FinTech.Core.Enums;
using FinTech.Core.Models;
using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTech.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SupportTicketController : CustomBaseController
    {
        private readonly ISupportTicketService _supportTicketService;

        public SupportTicketController(ISupportTicketService supportTicketService)
        {
            _supportTicketService = supportTicketService;
        }
        [HttpPost]
        [Authorize(Roles = RoleConstants.Customer)]
        public async Task<ActionResult<CustomResponse<SupportTicketCreatedDTO>>> Create(SupportTicketCreateDTO supportTicketCreateDTO)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return CreateActionResultInstance(await _supportTicketService.CreateAsync(userId,supportTicketCreateDTO));
        }
        [HttpGet]
        [Authorize(Roles = RoleConstants.SupportTicketAnalyst)]
        public async Task<ActionResult<CustomResponse<SupportTicketDTO>>> GetByWithoutPrioritizationStatus()
        {
            return CreateActionResultInstance(await _supportTicketService.GetByWithoutPrioritizationStatusAsync());
        }
        [HttpPost("{supportTicketId}")]
        [Authorize(Roles = RoleConstants.SupportTicketAnalyst)]
        public async Task<ActionResult<CustomResponse<NoContent>>> DeterminePriortyLevel(Guid supportTicketId , TicketPriorityLevel ticketPriorityLevel)
        {
            return CreateActionResultInstance(await _supportTicketService.DeterminePriortyLevelAsync(supportTicketId, ticketPriorityLevel));
        }
        [HttpGet]
        [Authorize(Roles = RoleConstants.CustomerSupport)]
        public async Task<ActionResult<CustomResponse<SupportTicketDTO>>> GetByPriorityStatus()
        {
            return CreateActionResultInstance(await _supportTicketService.GetByPriorityStatusAsync());
        }
        [HttpPost("{supportTicketId}")]
        [Authorize(Roles = RoleConstants.CustomerSupport)]
        public async Task<ActionResult<CustomResponse<SupportTicketCreatedDTO>>> Process(Guid supportTicketId)
        {
            return CreateActionResultInstance(await _supportTicketService.ProcessAsync(supportTicketId));
        }
    }
}
