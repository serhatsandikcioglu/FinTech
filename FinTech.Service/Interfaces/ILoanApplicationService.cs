using FinTech.Core.DTOs.LoanApplication;
using FinTech.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Interfaces
{
    public interface ILoanApplicationService
    {
        Task<CustomResponse<LoanApplicationDTO>> CreateAsync(Guid userId, LoanApplicationCreateDTO loanApplicationCreateDTO);
        Task<CustomResponse<NoContent>> LoanApplicationEvaluationAsync(Guid loanApplicationId, LoanApplicationEvaluationDTO loanApplicationEvaluationDTO);
        Task<CustomResponse<List<LoanApplicationDTO>>> GetAllByUserId(Guid userId);
    }
}
