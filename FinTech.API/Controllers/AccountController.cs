using FinTech.Core.Entities;
using FinTech.Service.Interfaces;
using FinTech.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FinTech.Core.DTOs.Account;
using FinTech.Core.DTOs.Balance;

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
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult<CustomResponse<AccountDTO>>> Create(AccountCreateDTO accountCreateDTO)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return CreateActionResultInstance(await _accountService.CreateAccountAccordingRulesAsync(userId, accountCreateDTO));
        }
        [HttpGet("{accountId}/balance")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult<CustomResponse<BalanceDTO>>> GetBalance(Guid accountId)
        {
            return CreateActionResultInstance( await _accountService.GetBalanceByAccountIdAsync(accountId));
        }
        [HttpPost("{accountId}/balance")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CustomResponse<BalanceDTO>>> UpdateBalance(Guid accountId, BalanceUpdateDTO balanceUpdateDTO)
        {
            return CreateActionResultInstance( await _accountService.UpdateBalanceAsync(accountId,balanceUpdateDTO));
        }
    }
}