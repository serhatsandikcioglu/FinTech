using FinTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Interfaces
{
    public interface IAutomaticPaymentRepository : IGenericRepository<AutomaticPayment>
    {
        Task<List<AutomaticPayment>> GetAllByUserIdAsync(Guid userId);
        Task<bool> IsExistAsync(Guid automaticPaymentId);
    }
}
