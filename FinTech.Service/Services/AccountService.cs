using AutoMapper;
using FinTech.Core.Entities;
using FinTech.Core.Enums;
using FinTech.Core.Interfaces;
using FinTech.Service.Interfaces;
using FinTech.Shared.Constans;
using FinTech.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FinTech.Core.DTOs.Account;
using FinTech.Core.DTOs.Balance;
using FinTech.Core.DTOs.AccountActivity;
using FinTech.Core.Constans;
using System.Security.Principal;

namespace FinTech.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountActivityService _accountActivityService;
        private static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly IHttpContextData _httpContextData;

        public AccountService(IMapper mapper, IUnitOfWork unitOfWork, IAccountActivityService accountActivityService, IHttpContextData httpContextData)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accountActivityService = accountActivityService;
            _httpContextData = httpContextData;
        }

        public async Task<CustomResponse<AccountDTO>> CreateAccountAccordingRulesAsync(AccountCreateDTO accountCreateDTO)
        {
            if (accountCreateDTO.Balance < AccountConstants.MinimumInitialBalance)
                return CustomResponse<AccountDTO>.Fail(StatusCodes.Status400BadRequest, $"{ErrorMessageConstants.InitialBalanceError}. Minimum Balance {AccountConstants.MinimumInitialBalance}");

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var accountResponse = await CreateAccountWithoutRulesAsync(Guid.Parse(_httpContextData.UserId!), accountCreateDTO);

                await AccountActivityCreateProcessAsync(accountCreateDTO.SenderAccountId,TransactionType.Withdrawal,accountCreateDTO.Balance);
                await AccountActivityCreateProcessAsync(accountResponse.Data.Id, TransactionType.Deposit, accountCreateDTO.Balance);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return CustomResponse<AccountDTO>.Success(StatusCodes.Status201Created, accountResponse.Data);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return CustomResponse<AccountDTO>.Fail(StatusCodes.Status400BadRequest, new List<string> { ex.Message });
            }
        }

        public async Task<CustomResponse<AccountDTO>> CreateAccountWithoutRulesAsync(Guid applicationUserId, AccountCreateDTO accountCreateDTO)
        {
            try
            {
                Account account = _mapper.Map<Account>(accountCreateDTO);
                account.ApplicationUserId = applicationUserId;
                account.Number = await CreateAccountNumberAsync();

                await _unitOfWork.AccountRepository.AddAsync(account);
                AccountDTO accountDTO = _mapper.Map<AccountDTO>(account);
                await _unitOfWork.SaveChangesAsync();
                return CustomResponse<AccountDTO>.Success(StatusCodes.Status201Created, accountDTO);
            }
            catch (Exception ex)
            {
                return CustomResponse<AccountDTO>.Fail(StatusCodes.Status400BadRequest, new List<string> { ex.Message });
            }
                
        }
        public async Task<CustomResponse<AccountDTO>> GetById(Guid accountId)
        {
            Account account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId);
            if (account == null)
                return CustomResponse<AccountDTO>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.AccountNotFound);

            AccountDTO accountDTO = _mapper.Map<AccountDTO>(account);
            return CustomResponse<AccountDTO>.Success(StatusCodes.Status200OK, accountDTO);
        }

        private async Task<string> CreateAccountNumberAsync()
        {
            int maxAccountNumberLength = 6;
            try
            {
                await _semaphoreSlim.WaitAsync();

                string biggestAccountNumber = await _unitOfWork.AccountRepository.GetBiggestAccountNumberAsync();
                BigInteger newAccountNumber = BigInteger.Parse(biggestAccountNumber ?? "0") + 1;
                string newAccountNumberString = $"{newAccountNumber:D}";
                newAccountNumberString = newAccountNumberString.PadLeft(maxAccountNumberLength, '0');
                return newAccountNumberString;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating account number", ex);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task<CustomResponse<BalanceDTO>> GetBalanceByAccountIdAsync(Guid accountId)
        {
            var accountOwnerId = await _unitOfWork.AccountRepository.GetUserIdById(accountId);
            if (!(Guid.Parse(_httpContextData.UserId!) == accountOwnerId) && !_httpContextData.UserRoleNames.Contains(RoleConstants.Admin) && !_httpContextData.UserRoleNames.Contains(RoleConstants.Manager))
                return CustomResponse<BalanceDTO>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.ForbiddenAccount);

            bool accountExist = await _unitOfWork.AccountRepository.AccountIsExistAsync(accountId);
            if (!accountExist)
                return CustomResponse<BalanceDTO>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.AccountNotFound);
            var accountActivities = await _unitOfWork.AccountActivityRepository.GetAllByAccountIdAsync(accountId);

            decimal totalBalance = 0;

            foreach (var activity in accountActivities)
            {
                if (activity.TransactionType == TransactionType.Deposit)
                {
                    totalBalance += activity.Amount;
                }
                else if (activity.TransactionType == TransactionType.Withdrawal)
                {
                    totalBalance -= activity.Amount;
                }
            }
            BalanceDTO balanceDTO = new BalanceDTO
            {
                Balance = totalBalance
            };
            return CustomResponse<BalanceDTO>.Success(StatusCodes.Status200OK, balanceDTO);
        }
        public async Task<CustomResponse<BalanceDTO>> UpdateBalanceAsync(Guid accountId, BalanceUpdateDTO balanceUpdateDTO)
        {
            Account account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId);
            if (account == null)
                return CustomResponse<BalanceDTO>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.AccountNotFound);
            if (balanceUpdateDTO.Balance < 0)
                return CustomResponse<BalanceDTO>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.MinBalanceError);

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var currentBalance = (await GetBalanceByAccountIdAsync(accountId)).Data.Balance;
                var changeAmount = balanceUpdateDTO.Balance - currentBalance;

                if (changeAmount > 0)
                {
                    await AccountActivityCreateProcessAsync(accountId, TransactionType.Deposit, changeAmount);
                }
                else if (changeAmount < 0)
                {
                    await AccountActivityCreateProcessAsync(accountId, TransactionType.Withdrawal, Math.Abs(changeAmount));
                }
                BalanceDTO balanceDTO = _mapper.Map<BalanceDTO>(balanceUpdateDTO);
                await _unitOfWork.CommitAsync();
                return CustomResponse<BalanceDTO>.Success(StatusCodes.Status200OK,balanceDTO);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return CustomResponse<BalanceDTO>.Fail(StatusCodes.Status400BadRequest, new List<string> { ex.Message });
            }
        }

        private async Task AccountActivityCreateProcessAsync(Guid accountId, TransactionType transactionType, decimal amount)
        {
            var accountActivityCreateDTO = new AccountActivityCreateDTO
            {
                Amount = amount,
                TransactionType = transactionType
            };

            var processResponse = await _accountActivityService.CreateAsync(accountId, accountActivityCreateDTO);

            if (processResponse.Error != null)
            {
                throw new Exception(processResponse.Error.Details[0].ToString());
            }
        }
    }
}
