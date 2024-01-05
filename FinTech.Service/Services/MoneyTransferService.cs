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
        private async Task<CustomResponse<NoContent>> PerformTransferAsync(MoneyTransfer moneyTransfer)
        {
            var checkResult = await PerformMoneyTransferChecksAsync(moneyTransfer.ReceiverAccountId, moneyTransfer.SenderAccountId, moneyTransfer.Amount);
            if (!checkResult.Succeeded)
                return checkResult;

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await AccountActivityCreateProcessAsync(moneyTransfer.SenderAccountId, TransactionType.Withdrawal, moneyTransfer.Amount);
                await AccountActivityCreateProcessAsync(moneyTransfer.ReceiverAccountId, TransactionType.Deposit, moneyTransfer.Amount);

                moneyTransfer.Date = DateTime.UtcNow;

                await _unitOfWork.MoneyTransferRepository.AddAsync(moneyTransfer);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return CustomResponse<NoContent>.Success(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, new List<string> { ex.Message });
            }
        }

        public async Task<CustomResponse<NoContent>> ExternalTransferAsync(ExternalTransferCreateDTO externalTransferCreateDTO)
        {
            Account receiverAccount = await _unitOfWork.AccountRepository.GetByAccountNumberAsync(externalTransferCreateDTO.ReceiverAccountNumber);
            if (receiverAccount == null)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.AccountNotFound);

            var receiverUser = receiverAccount.ApplicationUser;

            if (externalTransferCreateDTO.Name != receiverUser.Name || externalTransferCreateDTO.Surname != receiverUser.Surname)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.NameAndSurnameDontMatch);

            MoneyTransfer moneyTransfer = _mapper.Map<MoneyTransfer>(externalTransferCreateDTO);
            moneyTransfer.ReceiverAccountId = receiverAccount.Id;
            return await PerformTransferAsync(moneyTransfer);
        }

        public async Task<CustomResponse<NoContent>> InternalTransferAsync(InternalTransferCreateDTO internalTransferCreateDTO)
        {
            MoneyTransfer moneyTransfer = _mapper.Map<MoneyTransfer>(internalTransferCreateDTO);
            return await PerformTransferAsync(moneyTransfer);
        }

        private async Task<(bool IsWithinLimit , decimal RemainingLimit)> CalculateDailyTransferLimitAsync(Guid accountId , decimal amount)
        {
          var amountsSent = await  _unitOfWork.MoneyTransferRepository.GetDailyTransferAmountAsync(accountId,DateTime.UtcNow.Date);

            var totalAmountSent = amountsSent.Sum();
            decimal remainingLimit = MoneyTransferConstants.DailyTransferLimit - totalAmountSent;

            if (totalAmountSent + amount <= MoneyTransferConstants.DailyTransferLimit)
            {
                return (true,remainingLimit);
            }

            return (false,remainingLimit);
        }
        private async Task AccountActivityCreateProcessAsync(Guid accountId, TransactionType transactionType, decimal amount)
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
        private async Task<CustomResponse<NoContent>> PerformMoneyTransferChecksAsync(Guid receiverAccountId , Guid senderAccountId, decimal amount)
        {
            if (!await _unitOfWork.AccountRepository.AccountIsExistAsync(receiverAccountId) || !await _unitOfWork.AccountRepository.AccountIsExistAsync(senderAccountId))
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.AccountNotFound);

            if (amount > MoneyTransferConstants.PerTransactionTransferLimit)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.TransferLimitError);

            var (isWithinLimit, remainingLimit) = await CalculateDailyTransferLimitAsync(senderAccountId, amount);

            if (!isWithinLimit)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, $"{ErrorMessageConstants.DailyTransferLimitError}. Remaining limit: {remainingLimit}");

            return CustomResponse<NoContent>.Success(StatusCodes.Status200OK);
        }

    }
}
