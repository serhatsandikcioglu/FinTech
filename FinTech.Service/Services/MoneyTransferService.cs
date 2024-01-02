using AutoMapper;
using FinTech.Core.Entities;
using FinTech.Core.Enums;
using FinTech.Core.Interfaces;
using FinTech.Service.Interfaces;
using FinTech.Shared.Constans;
using FinTech.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using FinTech.Core.DTOs.MoneyTransfer;
using FinTech.Core.DTOs.AccountActivity;
using FinTech.Core.Constans;

namespace FinTech.Service.Services
{
    public class MoneyTransferService : IMoneyTransferService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountActivityService _accountActivityService;

        public MoneyTransferService(IUnitOfWork unitOfWork, IMapper mapper, IAccountActivityService accountActivityService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountActivityService = accountActivityService;
        }
        public async Task<CustomResponse<NoContent>> CreateAsync(MoneyTransferCreateDTO moneyTransferCreateDTO)
        {
            var chechResult = await PerformMoneyTransferChecksAsync(moneyTransferCreateDTO);
            if (!chechResult.Succeeded)
                return chechResult;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await AccountActivityCreateProcess(moneyTransferCreateDTO.SenderAccountId, TransactionType.Withdrawal, moneyTransferCreateDTO.Amount);
                Guid receiverAccountId = (await _unitOfWork.AccountRepository.GetByAccountNumberAsync(moneyTransferCreateDTO.ReceiverAccountNumber)).Id;
                await AccountActivityCreateProcess(receiverAccountId, TransactionType.Deposit, moneyTransferCreateDTO.Amount);
                MoneyTransfer moneyTransfer = _mapper.Map<MoneyTransfer>(moneyTransferCreateDTO);
                moneyTransfer.ReceiverAccountId = receiverAccountId;
                moneyTransfer.Date = DateTime.UtcNow;
                await _unitOfWork.MoneyTransferRepository.AddAsync(moneyTransfer);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return CustomResponse<NoContent>.Success(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return CustomResponse<NoContent>.Fail(StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }
        private async Task<bool> CalculateDailyTransferLimitAsync(Guid accountId , decimal amount)
        {
          var amountsSent = await  _unitOfWork.MoneyTransferRepository.GetDailyTransferAmountAsync(accountId,DateTime.UtcNow.Date);
            var totalAmountSent = amountsSent.Sum();
            if (totalAmountSent + amount <= MoneyTransferConstants.DailyTransferLimit)
            {
                return true;
            }
            return false;
        }
        private async Task AccountActivityCreateProcess(Guid accountId, TransactionType transactionType, decimal amount)
        {
            var accountActivityCreateDTO = new AccountActivityCreateDTO
            {
                Amount = amount,
                TransactionType = transactionType
            };

            var processResponse = await _accountActivityService.CreateAsync(accountId, accountActivityCreateDTO);

            if (!processResponse.Succeeded)
            {
                throw new Exception(processResponse.Error.Details[0].ToString());
            }
        }
        private async Task<CustomResponse<NoContent>> PerformMoneyTransferChecksAsync(MoneyTransferCreateDTO moneyTransferCreateDTO)
        {
            if (!await _unitOfWork.AccountRepository.AccountIsExistByAccountNumberAsync(moneyTransferCreateDTO.ReceiverAccountNumber))
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.AccountNotFound);

            if (moneyTransferCreateDTO.Amount > MoneyTransferConstants.PerTransactionTransferLimit)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.TransferLimitError);

            if (!await CalculateDailyTransferLimitAsync(moneyTransferCreateDTO.SenderAccountId, moneyTransferCreateDTO.Amount))
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.DailyTransferLimitError);

            return CustomResponse<NoContent>.Success(StatusCodes.Status200OK);
        }

    }
}
