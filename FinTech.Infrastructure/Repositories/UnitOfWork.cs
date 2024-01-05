using FinTech.Core.Interfaces;
using FinTech.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinTechDbContext _finTechDbContext;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(FinTechDbContext finTechDbContext, IServiceProvider serviceProvider)
        {
            _finTechDbContext = finTechDbContext;
            _serviceProvider = serviceProvider;
        }

        private IBillRepository _billRepository;
        private IAutomaticPaymentRepository _automaticPaymentRepository;
        private ISupportTicketRepository _supportTicketRepository;
        private IAccountRepository _accountRepository;
        private IAccountActivityRepository _accountActivityRepository;
        private IMoneyTransferRepository _moneyTransferRepository;
        private ILoanApplicationRepository _loanApplicationRepository;
        private IRepaymentPlanRepository _repaymentPlanRepository;
        public IBillRepository BillRepository
        {
            get
            {
                if (_billRepository == default(IBillRepository))
                    _billRepository = _serviceProvider.GetRequiredService<IBillRepository>();
                return _billRepository;
            }
        }
        public IAutomaticPaymentRepository AutomaticPaymentRepository
        {
            get
            {
                if (_automaticPaymentRepository == default(IAutomaticPaymentRepository))
                    _automaticPaymentRepository = _serviceProvider.GetRequiredService<IAutomaticPaymentRepository>();
                return _automaticPaymentRepository;
            }
        }
        public ISupportTicketRepository SupportTicketRepository
        {
            get
            {
                if (_supportTicketRepository == default(ISupportTicketRepository))
                    _supportTicketRepository = _serviceProvider.GetRequiredService<ISupportTicketRepository>();
                return _supportTicketRepository;
            }
        }
        public IAccountRepository AccountRepository
        {
            get
            {
                if (_accountRepository == default(IAccountRepository))
                    _accountRepository = _serviceProvider.GetRequiredService<IAccountRepository>();
                return _accountRepository;
            }
        }
        public IAccountActivityRepository AccountActivityRepository
        {
            get
            {
                if (_accountActivityRepository == default(IAccountActivityRepository))
                    _accountActivityRepository = _serviceProvider.GetRequiredService<IAccountActivityRepository>();
                return _accountActivityRepository;
            }
        }
        public IMoneyTransferRepository MoneyTransferRepository
        {
            get
            {
                if (_moneyTransferRepository == default(IMoneyTransferRepository))
                    _moneyTransferRepository = _serviceProvider.GetRequiredService<IMoneyTransferRepository>();
                return _moneyTransferRepository;
            }
        }
        public ILoanApplicationRepository LoanApplicationRepository
        {
            get
            {
                if (_loanApplicationRepository == default(ILoanApplicationRepository))
                    _loanApplicationRepository = _serviceProvider.GetRequiredService<ILoanApplicationRepository>();
                return _loanApplicationRepository;
            }
        }
        public IRepaymentPlanRepository RepaymentPlanRepository
        {
            get
            {
                if (_repaymentPlanRepository == default(IRepaymentPlanRepository))
                    _repaymentPlanRepository = _serviceProvider.GetRequiredService<IRepaymentPlanRepository>();
                return _repaymentPlanRepository;
            }
        }
        public IDbContextTransaction Transaction { get; private set; }

        public async Task BeginTransactionAsync()
        {
            Transaction = await _finTechDbContext.Database.BeginTransactionAsync();
        }
        public async Task CommitAsync()
        {
            if (Transaction == null)
                throw new DataException("No transaction is in progress");
            await Transaction.CommitAsync();
            Transaction = null;
        }
        public async Task RollbackAsync()
        {
            if (Transaction == null)
                throw new DataException("No transaction is in progress");
            await Transaction.RollbackAsync();
            Transaction = null;
        }
        public async Task SaveChangesAsync()
        {
           await _finTechDbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _finTechDbContext.Dispose();
        }
    }
}
