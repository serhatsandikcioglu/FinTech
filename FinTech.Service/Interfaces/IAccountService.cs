using FinTech.Core.DTOs.Account;
using FinTech.Core.DTOs.Balance;
using FinTech.Core.Entities;
using FinTech.Core.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Interfaces
{
    public interface IAccountService
    {
        Task<CustomResponse<AccountDTO>> CreateAccountWithoutRulesAsync(Guid userId, AccountCreateDTO accountCreateDTO);
        Task<CustomResponse<BalanceDTO>> GetBalanceByAccountIdAsync(Guid accountId);
        Task<CustomResponse<BalanceDTO>> UpdateBalanceAsync(Guid accountId, BalanceUpdateDTO balanceUpdateDTO);
        Task<CustomResponse<AccountDTO>> CreateAccountAccordingRulesAsync(AccountCreateDTO accountCreateDTO);
        Task<CustomResponse<AccountDTO>> GetById(Guid accountId);
    }
}
