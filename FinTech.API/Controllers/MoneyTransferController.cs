using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using FinTech.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinTech.Core.DTOs.MoneyTransfer;
using Microsoft.AspNetCore.Authorization;

namespace FinTech.API.Controllers
{
    [Route("api/moneyTransfers/[action]")]
    [ApiController]
    public class MoneyTransferController : CustomBaseController
    {
        private readonly IMoneyTransferService _moneyTransferService;

        public MoneyTransferController(IMoneyTransferService moneyTransferService)
        {
            _moneyTransferService = moneyTransferService;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult<CustomResponse<NoContent>>> ExternalTransfer(ExternalTransferCreateDTO externalTransferCreateDTO)
        {
            return CreateActionResultInstance( await _moneyTransferService.ExternalTransfer(externalTransferCreateDTO));
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult<CustomResponse<NoContent>>> InternalTransfer(InternalTransferCreateDTO internalTransferCreateDTO)
        {
            return CreateActionResultInstance(await _moneyTransferService.InternalTransfer(internalTransferCreateDTO));
        }
    }
}
