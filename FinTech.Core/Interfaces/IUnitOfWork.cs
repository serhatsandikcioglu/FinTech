using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinTech.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        IAccountRepository AccountRepository { get; }
        IAccountActivityRepository AccountActivityRepository { get; }
        IMoneyTransferRepository MoneyTransferRepository { get; }
        ILoanApplicationRepository LoanApplicationRepository { get; }
        IRepaymentPlanRepository RepaymentPlanRepository { get; }
        ISupportTicketRepository SupportTicketRepository { get; }
        IAutomaticPaymentRepository AutomaticPaymentRepository { get; }
        IBillRepository BillRepository { get; }
        Task SaveChangesAsync();
        IDbContextTransaction Transaction { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
