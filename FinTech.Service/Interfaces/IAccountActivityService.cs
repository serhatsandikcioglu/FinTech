using FinTech.Core.DTOs;
using FinTech.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Interfaces
{
    public interface IAccountActivityService
    {
        CustomResponse<AccountActivityDTO> Deposit(Guid accountId, AccountActivityCreateDTO accountActivityCreateDTO);
        CustomResponse<AccountActivityDTO> Withdrawal(Guid accountId, AccountActivityCreateDTO accountActivityCreateDTO);
    }
}
