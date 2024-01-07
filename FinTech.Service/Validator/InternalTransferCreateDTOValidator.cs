using FinTech.Core.DTOs.MoneyTransfer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Validator
{
    public class InternalTransferCreateDTOValidator : AbstractValidator<InternalTransferCreateDTO>
    {
        public InternalTransferCreateDTOValidator() 
        {
            RuleFor(x => x.SenderAccountId)
            .NotEmpty().WithMessage("SenderAccountId is required.");

            RuleFor(x => x.ReceiverAccountId)
                .NotEmpty().WithMessage("ReceiverAccountId is required.");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");
        }
    }
}
