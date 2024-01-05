using FinTech.Core.Entities;
using FinTech.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Interfaces
{
    public interface ILoanApplicationRepository : IGenericRepository<LoanApplication>
    {
        Task<List<PaymentStatus>>? GetPaymentStatusesByUserIdAsync(Guid applicationUserId);
        Task<List<LoanApplication>> GetAllByUserIdAsync(Guid UserId);
    }
}
