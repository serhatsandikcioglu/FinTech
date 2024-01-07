using FinTech.Core.DTOs.MoneyTransfer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Validator
{
    public class ExternalTransferCreateDTOValidator : AbstractValidator<ExternalTransferCreateDTO>
    {
        public ExternalTransferCreateDTOValidator() 
        {
            RuleFor(x => x.SenderAccountId)
           .NotEmpty().WithMessage("SenderAccountId is required.")
           .NotEqual(Guid.Empty).WithMessage("SenderAccountId must be a valid Guid.");

            RuleFor(x => x.ReceiverAccountNumber)
                .NotEmpty().WithMessage("ReceiverAccountNumber is required.")
                .Length(6);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .MaximumLength(50).WithMessage("Surname cannot exceed 50 characters.");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");
        }
    }
}
