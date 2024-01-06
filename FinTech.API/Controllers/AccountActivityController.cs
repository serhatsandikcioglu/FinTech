using FinTech.Core.Entities;
using FinTech.Service.Interfaces;
using FinTech.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinTech.Core.DTOs.AccountActivity;
using Microsoft.AspNetCore.Authorization;
using FinTech.Core.Constans;

namespace FinTech.API.Controllers
{
    [Route("api/accountActivities")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer" , Roles = $"{RoleConstants.Customer},{RoleConstants.Admin},{RoleConstants.Manager}")]
    public class AccountActivityController : CustomBaseController
    {
        private readonly IAccountActivityService _accountActivityService;

        public AccountActivityController(IAccountActivityService accountActivityService)
        {
            _accountActivityService = accountActivityService;
        }
        [HttpPost("{accountId}")]
        public async Task<ActionResult<CustomResponse<AccountActivityDTO>>> Create(Guid accountId, AccountActivityCreateDTO accountActivityCreateDTO)
        {
            return CreateActionResultInstance( await _accountActivityService.CreateAsync(accountId, accountActivityCreateDTO));
        }
    }
}
