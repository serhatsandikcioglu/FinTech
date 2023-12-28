using AutoMapper;
using FinTech.Core.DTOs;
using FinTech.Core.Entities;
using FinTech.Core.Interfaces;
using FinTech.Service.Interfaces;
using FinTech.Shared.Constans;
using FinTech.Shared.Models;
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

namespace FinTech.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public CustomResponse< AccountDTO> Create(AccountCreateDTO accountCreateDTO)
        {
            if (accountCreateDTO.Balance < AccountConstants.MinimumInitialBalance) 
                return CustomResponse<AccountDTO>.Fail(StatusCodes.Status400BadRequest, "Initial account balance must be above the account opening limit.");
            Account account = _mapper.Map<Account>(accountCreateDTO);
            account.Number = CreateAccountNumber();
            _unitOfWork.AccountRepository.Add(account);
            _unitOfWork.SaveChanges();
            AccountDTO accountDTO = _mapper.Map<AccountDTO>(account);
            return CustomResponse<AccountDTO>.Success(StatusCodes.Status201Created, accountDTO);
        }

        public string CreateAccountNumber()
        {
            int maxAccountNumberLength = 6;
            _unitOfWork.BeginTransactionAsync();
                try
                {
                    string biggestAccountNumber = _unitOfWork.AccountRepository.GetBiggestAccountNumber();
                    BigInteger newAccountNumber = BigInteger.Parse(biggestAccountNumber ?? "0") + 1;
                    string newAccountNumberString = $"{newAccountNumber:D}";
                    newAccountNumberString = newAccountNumberString.PadLeft(maxAccountNumberLength, '0');
                    _unitOfWork.CommitAsync();
                    return newAccountNumberString;
                }
                catch (Exception ex)
                {
                    _unitOfWork.RollbackAsync();
                    throw;
                }
        }

        public CustomResponse<BalanceDTO> GetBalanceByAccountId(Guid accountId)
        {
            bool accountExist = _unitOfWork.AccountRepository.AccountIsExist(accountId);
            if (!accountExist)
                 return CustomResponse<BalanceDTO>.Fail(StatusCodes.Status404NotFound, "Account Not Found");
            
            BalanceDTO balanceDTO = new BalanceDTO();
            balanceDTO.Balance =  _unitOfWork.AccountRepository.GetBalanceByAccountId(accountId);
            return CustomResponse<BalanceDTO>.Success(StatusCodes.Status200OK, balanceDTO);
            
        }
        public CustomResponse<NoContent> Update(Guid accountId, BalanceUpdateDTO balanceUpdateDTO)
        {
            Account account = _unitOfWork.AccountRepository.GetById(accountId);
            if (account == null)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound, "Account Not Found");
            if (balanceUpdateDTO.Balance < 0)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, "Balance cannot be less than 0");
            account.Balance = balanceUpdateDTO.Balance;
            _unitOfWork.SaveChanges();
            return CustomResponse<NoContent>.Success(StatusCodes.Status200OK);
        }
    }
}
