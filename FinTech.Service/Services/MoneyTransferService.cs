using AutoMapper;
using FinTech.Core.DTOs;
using FinTech.Core.Entities;
using FinTech.Core.Interfaces;
using FinTech.Service.Interfaces;
using FinTech.Shared.Constans;
using FinTech.Shared.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Services
{
    public class MoneyTransferService : IMoneyTransferService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MoneyTransferService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public CustomResponse<NoContent> Create(MoneyTransferCreateDTO moneyTransferCreateDTO)
        {
            Account senderAccount = _unitOfWork.AccountRepository.GetById(moneyTransferCreateDTO.SenderAccountId);
            if (senderAccount == null)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound, "Account Not Found");
            if (senderAccount.Balance < moneyTransferCreateDTO.Amount)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, "Insufficient Balance");
            if (moneyTransferCreateDTO.Amount > MoneyTransferConstants.PerTransactionTransferLimit)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, "Transfer limit exceeded");
            if (!CalculateDailyTransferLimit(moneyTransferCreateDTO.SenderAccountId, moneyTransferCreateDTO.Amount))
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, "Daily Transfer limit exceeded");
            try
            {
                _unitOfWork.BeginTransactionAsync();
                senderAccount.Balance = senderAccount.Balance - moneyTransferCreateDTO.Amount;
                _unitOfWork.SaveChanges();
                Account receiverAccount = _unitOfWork.AccountRepository.GetByAccountNumber(moneyTransferCreateDTO.ReceiverAccountNumber);
                if (receiverAccount == null)
                    return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound, "Account Not Found");
                receiverAccount.Balance = receiverAccount.Balance + moneyTransferCreateDTO.Amount;
                MoneyTransfer moneyTransfer = _mapper.Map<MoneyTransfer>(moneyTransferCreateDTO);
                moneyTransfer.Date = DateTime.UtcNow;
                _unitOfWork.MoneyTransferRepository.Add(moneyTransfer);
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitAsync();
                return CustomResponse<NoContent>.Success(StatusCodes.Status200OK);
            }
            catch (Exception)
            {
                _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public bool CalculateDailyTransferLimit(Guid accountId , decimal amount)
        {
          var amounts =   _unitOfWork.MoneyTransferRepository.GetDailyTransferAmount(accountId,DateTime.UtcNow.Date);
            var totalAmount = amounts.Sum();
            if (totalAmount + amount < MoneyTransferConstants.DailyTransferLimit)
            {
                return true;
            }
            return false;
        }
    }
}
