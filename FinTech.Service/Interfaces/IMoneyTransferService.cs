using FinTech.Core.DTOs.MoneyTransfer;
using FinTech.Core.Entities;
using FinTech.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Interfaces
{
    public interface IMoneyTransferService
    {
        Task<CustomResponse<NoContent>> CreateAsync(MoneyTransferCreateDTO moneyTransferCreateDTO);
    }
}
