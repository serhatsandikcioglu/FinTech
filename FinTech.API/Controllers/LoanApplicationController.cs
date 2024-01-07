using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using FinTech.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FinTech.Core.DTOs.LoanApplication;
using FinTech.Core.Constans;
using FinTech.Core.Enums;

namespace FinTech.API.Controllers
{
    [Route("api/loanApplications")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LoanApplicationController : CustomBaseController
    {
        private readonly ILoanApplicationService _loanApplicationService;

        public LoanApplicationController(ILoanApplicationService loanApplicationService)
        {
            _loanApplicationService = loanApplicationService;
        }

        [HttpPost]

        [Authorize(Roles = $"{RoleConstants.Customer},{RoleConstants.Admin}")]
        public async Task<ActionResult<CustomResponse<LoanApplicationDTO>>> Create(LoanApplicationCreateDTO loanApplicationCreateDTO)
        {
            return CreateActionResultInstance(await _loanApplicationService.CreateAsync(loanApplicationCreateDTO));
        }
        [HttpGet("byUser")]
        [Authorize(Roles = $"{RoleConstants.Customer},{RoleConstants.Admin}")]
        public async Task<ActionResult<CustomResponse<List<LoanApplicationDTO>>>> GetAllByUserId()
        {
            return CreateActionResultInstance(await _loanApplicationService.GetAllByUserIdAsync());
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConstants.LoanOfficer},{RoleConstants.Admin}")]
        public async Task<ActionResult<CustomResponse<List<LoanApplicationDTO>>>> GetAll()
        {
            return CreateActionResultInstance(await _loanApplicationService.GetAllAsync());
        }
        [HttpPut("evaluation/{loanApplicationId}")]
        [Authorize(Roles = $"{RoleConstants.LoanOfficer},{RoleConstants.Admin}")]
        public async Task<ActionResult<CustomResponse<NoContent>>> LoanApplicationEvaluation(Guid loanApplicationId , LoanApllicationStatus loanApllicationStatus)
        {
            return CreateActionResultInstance( await _loanApplicationService.LoanApplicationEvaluationAsync(loanApplicationId, loanApllicationStatus));
        }
    }
}
