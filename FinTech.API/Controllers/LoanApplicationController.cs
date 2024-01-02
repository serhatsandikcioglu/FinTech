using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using FinTech.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FinTech.Core.DTOs.LoanApplication;

namespace FinTech.API.Controllers
{
    [Route("api/loanApplications")]
    [ApiController]
    public class LoanApplicationController : CustomBaseController
    {
        private readonly ILoanApplicationService _loanApplicationService;

        public LoanApplicationController(ILoanApplicationService loanApplicationService)
        {
            _loanApplicationService = loanApplicationService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult<CustomResponse<LoanApplicationDTO>>> Create(LoanApplicationCreateDTO loanApplicationCreateDTO)
        {
            Guid customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return CreateActionResultInstance(await _loanApplicationService.CreateAsync(customerId, loanApplicationCreateDTO));
        }
        [HttpPut("{loanApplicationId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin,loanOfficer")]
        public async Task<ActionResult<CustomResponse<NoContent>>> LoanApplicationEvaluation(Guid loanApplicationId , LoanApplicationEvaluationDTO loanApplicationEvaluationDTO)
        {
            return CreateActionResultInstance( await _loanApplicationService.LoanApplicationEvaluationAsync(loanApplicationId,loanApplicationEvaluationDTO));
        }
    }
}
