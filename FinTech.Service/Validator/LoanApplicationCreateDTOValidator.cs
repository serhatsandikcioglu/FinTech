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
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(x => x.MaturityTerm)
                .NotEmpty().WithMessage("MaturityTerm is required.")
                .GreaterThan(0).WithMessage("MaturityTerm must be greater than 0.");

            RuleFor(x => x.MonthlyIncome)
                .NotEmpty().WithMessage("MonthlyIncome is required.")
                .GreaterThan(0).WithMessage("MonthlyIncome must be greater than 0.");
        }
    }
}
