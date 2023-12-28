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
        void SaveChanges();
        IDbContextTransaction Transaction { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
