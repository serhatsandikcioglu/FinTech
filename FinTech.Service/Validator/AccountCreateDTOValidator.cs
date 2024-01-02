using FinTech.Core.DTOs.Account;
using FinTech.Shared.Constans;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Validator
{
    public class AccountCreateDTOValidator : AbstractValidator<AccountCreateDTO>
    {
        public AccountCreateDTOValidator()
        {
            RuleFor(x=>x.Balance).NotEmpty().GreaterThanOrEqualTo(AccountConstants.MinimumInitialBalance).WithMessage($"Initial balance must be at least {AccountConstants.MinimumInitialBalance}.");
        }
    }
}
