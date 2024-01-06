using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using FinTech.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinTech.Core.DTOs.MoneyTransfer;
using Microsoft.AspNetCore.Authorization;
using FinTech.Core.Constans;

namespace FinTech.API.Controllers
{
    [Route("api/moneyTransfers")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer" , Roles = $"{RoleConstants.Customer},{RoleConstants.Admin}")]
    public class MoneyTransferController : CustomBaseController
    {
        private readonly IMoneyTransferService _moneyTransferService;

        public MoneyTransferController(IMoneyTransferService moneyTransferService)
        {
            _moneyTransferService = moneyTransferService;
        }
        [HttpPost("externalTransfer")]
        public async Task<ActionResult<CustomResponse<NoContent>>> ExternalTransfer(ExternalTransferCreateDTO externalTransferCreateDTO)
        {
            return CreateActionResultInstance( await _moneyTransferService.ExternalTransferAsync(externalTransferCreateDTO));
        }
        [HttpPost("internalTransfer")]
        public async Task<ActionResult<CustomResponse<NoContent>>> InternalTransfer(InternalTransferCreateDTO internalTransferCreateDTO)
        {
            return CreateActionResultInstance(await _moneyTransferService.InternalTransferAsync(internalTransferCreateDTO));
        }
    }
}
