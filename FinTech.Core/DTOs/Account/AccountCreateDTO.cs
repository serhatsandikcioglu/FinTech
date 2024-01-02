using FinTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.Account
{
    public class AccountCreateDTO
    {
        public decimal Balance { get; set; }
        public Guid SenderAccountId { get; set; }
    }
}
