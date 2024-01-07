using FinTech.Core.DTOs.LoanApplication;
using FinTech.Core.Enums;
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
        Task<CustomResponse<LoanApplicationDTO>> CreateAsync(LoanApplicationCreateDTO loanApplicationCreateDTO);
        Task<CustomResponse<NoContent>> LoanApplicationEvaluationAsync(Guid loanApplicationId, LoanApllicationStatus loanApllicationStatus);
        Task<CustomResponse<List<LoanApplicationDTO>>> GetAllByUserIdAsync();
        Task<CustomResponse<List<LoanApplicationDTO>>> GetAllAsync();
    }
}
