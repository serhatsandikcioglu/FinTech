using AutoMapper;
using FinTech.Core.DTOs;
using FinTech.Core.Entities;
using FinTech.Core.Enums;
using FinTech.Core.Interfaces;
using FinTech.Service.Interfaces;
using FinTech.Shared.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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

        public CustomResponse< AccountActivityDTO> Deposit(Guid accountId ,AccountActivityCreateDTO accountActivityCreateDTO)
        {
                Account account = _unitOfWork.AccountRepository.GetById(accountId);
                if (account == null)
                    return CustomResponse<AccountActivityDTO>.Fail(StatusCodes.Status404NotFound, "Account Not Found");
            _semaphoreSlim.WaitAsync();
            try
            {
                _unitOfWork.BeginTransactionAsync();
                account.Balance += accountActivityCreateDTO.Amount;
                AccountActivity accountActivity = _mapper.Map<AccountActivity>(accountActivityCreateDTO);
                accountActivity.TransactionType = TransactionType.Deposit;
                accountActivity.Date = DateTime.UtcNow;
                accountActivity.AccountId = account.Id;
                _unitOfWork.AccountActivityRepository.Add(accountActivity);
                _unitOfWork.SaveChanges();
                AccountActivityDTO accountActivityDTO = _mapper.Map<AccountActivityDTO>(accountActivity);
                _unitOfWork.CommitAsync();
                return CustomResponse<AccountActivityDTO>.Success(StatusCodes.Status200OK, accountActivityDTO);
            }
            catch (Exception)
            {
                _unitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public CustomResponse< AccountActivityDTO> Withdrawal(Guid accountId, AccountActivityCreateDTO accountActivityCreateDTO)
        {
                Account account = _unitOfWork.AccountRepository.GetById(accountId);
                if (account == null)
                    return CustomResponse<AccountActivityDTO>.Fail(StatusCodes.Status404NotFound, "Account Not Found");
                if (account.Balance < accountActivityCreateDTO.Amount)
                    return CustomResponse<AccountActivityDTO>.Fail(StatusCodes.Status400BadRequest, "Insufficient funds");
            _semaphoreSlim.WaitAsync();
            try
            {
                _unitOfWork.BeginTransactionAsync();
                account.Balance = account.Balance -  accountActivityCreateDTO.Amount;
                AccountActivity accountActivity = _mapper.Map<AccountActivity>(accountActivityCreateDTO);
                accountActivity.TransactionType = TransactionType.Withdrawal;
                accountActivity.Date = DateTime.UtcNow;
                accountActivity.AccountId = account.Id;
                _unitOfWork.AccountActivityRepository.Add(accountActivity);
                _unitOfWork.SaveChanges();
                AccountActivityDTO accountActivityDTO = _mapper.Map<AccountActivityDTO>(accountActivity);
                _unitOfWork.CommitAsync();
                return CustomResponse<AccountActivityDTO>.Success(StatusCodes.Status200OK, accountActivityDTO);
            }
            catch (Exception)
            {
                _unitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
