using FinTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs
{
    public class AccountDTO
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public Guid ApplicationUserId { get; set; }
        public decimal Balance { get; set; }
    }
}
