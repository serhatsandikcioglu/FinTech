using FinTech.Core.DTOs;
using FinTech.Core.Entities;
using FinTech.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Interfaces
{
    public interface IMoneyTransferService
    {
        CustomResponse<NoContent> Create(MoneyTransferCreateDTO moneyTransferCreateDTO);
        bool CalculateDailyTransferLimit(Guid accountId, decimal amount);
    }
}
