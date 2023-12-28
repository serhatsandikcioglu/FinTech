using FinTech.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Validator
{
    public class BalanceUpdateDTOValidator : AbstractValidator<BalanceUpdateDTO>
    {
        public BalanceUpdateDTOValidator() 
        {
            RuleFor(x=>x.Balance).NotEmpty().GreaterThanOrEqualTo(0).WithMessage("Balance cannot be less than 0");
        }
    }
}
