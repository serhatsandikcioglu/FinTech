using AutoMapper;
using FinTech.Core.DTOs.SupportTicket;
using FinTech.Core.Entities;
using FinTech.Core.Interfaces;
using FinTech.Core.Models;
using FinTech.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using FinTech.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTech.Core.Constans;

namespace FinTech.Service.Services
{
    public class SupportTicketService : ISupportTicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupportTicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CustomResponse<SupportTicketCreatedDTO>> Create(Guid userId , SupportTicketCreateDTO supportTicketCreateDTO)
        {
            SupportTicket supportTicket = _mapper.Map<SupportTicket>(supportTicketCreateDTO);
            supportTicket.ApplicationUserId = userId;
            supportTicket.CreatedDate = DateTime.UtcNow;
            supportTicket.Status = TicketStatus.Pending;

            await _unitOfWork.SupportTicketRepository.AddAsync(supportTicket);
            await _unitOfWork.SaveChangesAsync();
            SupportTicketCreatedDTO supportTicketCreatedDTO = _mapper.Map<SupportTicketCreatedDTO>(supportTicket);
            return CustomResponse<SupportTicketCreatedDTO>.Success(StatusCodes.Status201Created, supportTicketCreatedDTO);
        }
        public async Task<CustomResponse<SupportTicketDTO>> GetByWithoutPrioritizationStatus()
        {
            SupportTicket supportTicket = await _unitOfWork.SupportTicketRepository.GetOldestUnprioritizedSupportRequest();

            if (supportTicket == null)
                return CustomResponse<SupportTicketDTO>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.SupportTicketNotFound);

            SupportTicketDTO supportTicketDTO = _mapper.Map<SupportTicketDTO>(supportTicket);
            return CustomResponse<SupportTicketDTO>.Success(StatusCodes.Status200OK,supportTicketDTO);
        }
        public async Task<CustomResponse<NoContent>> DeterminePriortyLevel(Guid supportTicketId, TicketPriorityLevel ticketPriorityLevel)
        {
            SupportTicket supportTicket = await _unitOfWork.SupportTicketRepository.GetById(supportTicketId);

            if (supportTicket == null)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.SupportTicketNotFound);

            if (supportTicket.PriorityLevel != null)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.SupportTicketPrioritized);

            supportTicket.PriorityLevel = ticketPriorityLevel;
            await _unitOfWork.SaveChangesAsync();
            return CustomResponse<NoContent>.Success(StatusCodes.Status200OK);
        }
        public async Task<CustomResponse<SupportTicketDTO>> GetByPriorityStatus()
        {
            SupportTicket supportTicket = await _unitOfWork.SupportTicketRepository.GetOldestPendingPrioritySupportRequest();

            if (supportTicket == null)
                return CustomResponse<SupportTicketDTO>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.SupportTicketNotFound);

            SupportTicketDTO supportTicketDTO = _mapper.Map<SupportTicketDTO>(supportTicket);
            return CustomResponse<SupportTicketDTO>.Success(StatusCodes.Status200OK, supportTicketDTO);
        }
    }
}
