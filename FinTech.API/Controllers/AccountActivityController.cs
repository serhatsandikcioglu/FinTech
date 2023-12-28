using FinTech.Core.DTOs;
using FinTech.Core.Entities;
using FinTech.Service.Interfaces;
using FinTech.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountActivityController : CustomBaseController
    {
        private readonly IAccountActivityService _accountActivityService;

        public AccountActivityController(IAccountActivityService accountActivityService)
        {
            _accountActivityService = accountActivityService;
        }
        [HttpPost("{accountId}")]
        public async Task<ActionResult<CustomResponse<AccountActivityDTO>>> Withdrawal(Guid accountId, AccountActivityCreateDTO accountActivityCreateDTO)
        {
            return CreateActionResultInstance( _accountActivityService.Withdrawal(accountId, accountActivityCreateDTO));
        }
        [HttpPost("{accountId}")]
        public async Task<ActionResult<CustomResponse<AccountActivityDTO>>> Deposit(Guid accountId, AccountActivityCreateDTO accountActivityCreateDTO)
        {
            return CreateActionResultInstance(_accountActivityService.Deposit(accountId, accountActivityCreateDTO));
        }
    }
}
