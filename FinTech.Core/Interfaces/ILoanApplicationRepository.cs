using FinTech.Core.Entities;
using FinTech.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Interfaces
{
    public interface ILoanApplicationRepository
    {
        Task AddAsync(LoanApplication loanApplication);
        Task<List<PaymentStatus>>? GetPaymentStatusesByUserIdAsync(Guid applicationUserId);
        Task<LoanApplication> GetByIdAsync(Guid loanApplicationId);
    }
}
