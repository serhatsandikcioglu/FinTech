using FinTech.Core.DTOs;
using FinTech.Core.Entities;
using FinTech.Shared.Models;
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
        CustomResponse<AccountDTO> Create(AccountCreateDTO accountCreateDTO);
        string CreateAccountNumber();
        CustomResponse<BalanceDTO> GetBalanceByAccountId(Guid accountId);
        CustomResponse<NoContent> Update(Guid accountId, BalanceUpdateDTO balanceUpdateDTO);
    }
}
