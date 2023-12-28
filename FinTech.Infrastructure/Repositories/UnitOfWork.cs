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


        private IAccountRepository _accountRepository;
        private IAccountActivityRepository _accountActivityRepository;
        private IMoneyTransferRepository _moneyTransferRepository;
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

        public void SaveChanges()
        {
            _finTechDbContext.SaveChanges();
        }

        public void Dispose()
        {
            _finTechDbContext.Dispose();
        }
    }
}
