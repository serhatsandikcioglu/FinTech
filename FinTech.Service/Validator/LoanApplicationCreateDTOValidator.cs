using FinTech.Core.DTOs.LoanApplication;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Validator
{
    public class LoanApplicationCreateDTOValidator : AbstractValidator<LoanApplicationCreateDTO>
    {
        public LoanApplicationCreateDTOValidator()
        {
            RuleFor(x=>x.MonthlyIncome).NotEmpty().GreaterThanOrEqualTo(1);
        }
    }
}
