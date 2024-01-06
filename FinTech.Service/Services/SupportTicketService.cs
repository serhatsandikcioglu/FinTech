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
        private readonly IHttpContextData _httpContextData;

        public SupportTicketService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextData httpContextData)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextData = httpContextData;
        }
        public async Task<CustomResponse<SupportTicketCreatedDTO>> CreateAsync(SupportTicketCreateDTO supportTicketCreateDTO)
        {
            SupportTicket supportTicket = _mapper.Map<SupportTicket>(supportTicketCreateDTO);
            supportTicket.ApplicationUserId = Guid.Parse(_httpContextData.UserId!);
            supportTicket.CreatedDate = DateTime.UtcNow;
            supportTicket.Status = TicketStatus.Pending;

            await _unitOfWork.SupportTicketRepository.AddAsync(supportTicket);
            SupportTicketCreatedDTO supportTicketCreatedDTO = _mapper.Map<SupportTicketCreatedDTO>(supportTicket);
            await _unitOfWork.SaveChangesAsync();
            return CustomResponse<SupportTicketCreatedDTO>.Success(StatusCodes.Status201Created, supportTicketCreatedDTO);
        }
        public async Task<CustomResponse<SupportTicketDTO>> GetByWithoutPrioritizationStatusAsync()
        {
            SupportTicket supportTicket = await _unitOfWork.SupportTicketRepository.GetOldestUnprioritizedSupportRequestAsync();

            if (supportTicket == null)
                return CustomResponse<SupportTicketDTO>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.SupportTicketNotFound);

            SupportTicketDTO supportTicketDTO = _mapper.Map<SupportTicketDTO>(supportTicket);
            return CustomResponse<SupportTicketDTO>.Success(StatusCodes.Status200OK,supportTicketDTO);
        }
        public async Task<CustomResponse<NoContent>> DeterminePriortyLevelAsync(Guid supportTicketId, TicketPriorityLevel ticketPriorityLevel)
        {
            SupportTicket supportTicket = await _unitOfWork.SupportTicketRepository.GetByIdAsync(supportTicketId);

            if (supportTicket == null)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.SupportTicketNotFound);

            if (supportTicket.PriorityLevel != null)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.SupportTicketPrioritized);

            supportTicket.PriorityLevel = ticketPriorityLevel;
            await _unitOfWork.SaveChangesAsync();
            return CustomResponse<NoContent>.Success(StatusCodes.Status200OK);
        }
        public async Task<CustomResponse<SupportTicketDTO>> GetByPriorityStatusAsync()
        {
            SupportTicket supportTicket = await _unitOfWork.SupportTicketRepository.GetOldestPendingPrioritySupportRequestAsync();

            if (supportTicket == null)
                return CustomResponse<SupportTicketDTO>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.SupportTicketNotFound);

            SupportTicketDTO supportTicketDTO = _mapper.Map<SupportTicketDTO>(supportTicket);
            return CustomResponse<SupportTicketDTO>.Success(StatusCodes.Status200OK, supportTicketDTO);
        }
        public async Task<CustomResponse<NoContent>> ProcessAsync(Guid supportTicketId)
        {
            SupportTicket supportTicket = await _unitOfWork.SupportTicketRepository.GetByIdAsync(supportTicketId);

            if (supportTicket == null)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.SupportTicketNotFound);

            if (supportTicket.Status != TicketStatus.Pending)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.SupportTicketProcessed);

            supportTicket.Status = TicketStatus.Processing;
            await _unitOfWork.SaveChangesAsync();
            return CustomResponse<NoContent>.Success(StatusCodes.Status200OK);

        }
    }
}
