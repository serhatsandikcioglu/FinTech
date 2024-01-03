using AutoMapper;
using FinTech.Core.Entities;
using FinTech.Core.Enums;
using FinTech.Core.Interfaces;
using FinTech.Service.Interfaces;
using FinTech.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FinTech.Core.DTOs.AccountActivity;
using FinTech.Core.Constans;
using System.Reflection.Metadata;

namespace FinTech.Service.Services
{
    public class AccountActivityService : IAccountActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        public AccountActivityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomResponse<AccountActivityDTO>> CreateAsync(Guid accountId, AccountActivityCreateDTO accountActivityCreateDTO)
        {
            Account account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId);
            if (account == null)
                return CustomResponse<AccountActivityDTO>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.AccountNotFound );
            if (accountActivityCreateDTO.TransactionType == TransactionType.Withdrawal && accountActivityCreateDTO.Amount >(await GetBalanceAsync(accountId)) )
                return CustomResponse<AccountActivityDTO>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.InsufficientFunds);
            await _semaphoreSlim.WaitAsync();
            try
            {
                AccountActivity accountActivity = _mapper.Map<AccountActivity>(accountActivityCreateDTO);
                accountActivity.Date = DateTime.UtcNow;
                accountActivity.AccountId = account.Id;
                AccountActivityDTO accountActivityDTO = _mapper.Map<AccountActivityDTO>(accountActivity);
                await _unitOfWork.AccountActivityRepository.AddAsync(accountActivity);
                await _unitOfWork.SaveChangesAsync();
                return CustomResponse<AccountActivityDTO>.Success(StatusCodes.Status201Created, accountActivityDTO);
            }
            catch (Exception ex)
            {
                return CustomResponse<AccountActivityDTO>.Fail(StatusCodes.Status400BadRequest, new List<string> { ex.Message });
            }
            finally
            {
                  _semaphoreSlim.Release();
            }
        }
        private async Task<decimal> GetBalanceAsync(Guid accountId)
        {
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

            return totalBalance;
        }
    }
}
