using FinTech.Core.DTOs.AutomaticPayment;
using FinTech.Core.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Validator
{
    public class AutomaticPaymentCreateDTOValidator : AbstractValidator<AutomaticPaymentCreateDTO>
    {
        public AutomaticPaymentCreateDTOValidator()
        {
            RuleFor(x => x.BillNumber).NotEmpty().MaximumLength(15);
        }
    }
}
