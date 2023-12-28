using FinTech.Core.DTOs;
using FinTech.Core.Entities;
using FinTech.Service.Interfaces;
using FinTech.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : CustomBaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost]
        public async Task<ActionResult<CustomResponse<AccountDTO>>> Create(AccountCreateDTO accountCreateDTO)
        {
            return CreateActionResultInstance(_accountService.Create(accountCreateDTO));
        }
        [HttpGet("{accountId}/balance")]
        public async Task<ActionResult<CustomResponse<BalanceDTO>>> GetBalance(Guid accountId)
        {
            return CreateActionResultInstance(_accountService.GetBalanceByAccountId(accountId));
        }
        [HttpPut("{accountId}/balance")]
        public async Task<ActionResult<CustomResponse<NoContent>>> UpdateBalance(Guid accountId, BalanceUpdateDTO balanceUpdateDTO)
        {
            return CreateActionResultInstance(_accountService.Update(accountId,balanceUpdateDTO));
        }
    }
}
