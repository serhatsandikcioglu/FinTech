using FinTech.Core.DTOs;
using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using FinTech.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers
{
    [Route("api/moneyTransfers")]
    [ApiController]
    public class MoneyTransferController : CustomBaseController
    {
        private readonly IMoneyTransferService _moneyTransferService;

        public MoneyTransferController(IMoneyTransferService moneyTransferService)
        {
            _moneyTransferService = moneyTransferService;
        }
        [HttpPost]
        public async Task<ActionResult<CustomResponse<NoContent>>> Create(MoneyTransferCreateDTO moneyTransferCreateDTO)
        {
            return CreateActionResultInstance(_moneyTransferService.Create(moneyTransferCreateDTO));
        }
    }
}
