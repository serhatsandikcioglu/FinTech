using FinTech.Core.Entities;
using FinTech.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.LoanApplication
{
    public class LoanApplicationCreateDTO
    {
        public decimal Amount { get; set; }
        public int MaturityTerm { get; set; }
        public decimal MonthlyIncome { get; set; }

    }
}
