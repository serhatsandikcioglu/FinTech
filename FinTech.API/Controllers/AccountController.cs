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
using FinTech.Core.Constans;

namespace FinTech.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountController : CustomBaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Customer},{RoleConstants.Admin}")]
        public async Task<ActionResult<CustomResponse<AccountDTO>>> Create(AccountCreateDTO accountCreateDTO)
        {
            return CreateActionResultInstance(await _accountService.CreateAccountAccordingRulesAsync(accountCreateDTO));
        }
        [HttpGet("balance/{accountId}")]
        [Authorize(Roles = $"{RoleConstants.Customer},{RoleConstants.Admin}")]
        public async Task<ActionResult<CustomResponse<BalanceDTO>>> GetBalance(Guid accountId)
        {
            return CreateActionResultInstance( await _accountService.GetBalanceByAccountIdAsync(accountId));
        }
        [HttpPost("balance/{accountId}")]
        [Authorize(Roles = $"{RoleConstants.Manager},{RoleConstants.Admin}")]
        public async Task<ActionResult<CustomResponse<BalanceDTO>>> UpdateBalance(Guid accountId, BalanceUpdateDTO balanceUpdateDTO)
        {
            return CreateActionResultInstance( await _accountService.UpdateBalanceAsync(accountId,balanceUpdateDTO));
        }
        [HttpGet("{accountId}")]
        [Authorize(Roles = $"{RoleConstants.Manager},{RoleConstants.Admin}")]
        public async Task<ActionResult<CustomResponse<Account>>> GetById(Guid accountId)
        {
            return CreateActionResultInstance(await _accountService.GetById(accountId));
        }
    }
}