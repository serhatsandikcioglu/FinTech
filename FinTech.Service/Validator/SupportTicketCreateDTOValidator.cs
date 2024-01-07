using FinTech.Core.DTOs.SupportTicket;
using FinTech.Core.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Validator
{
    public class SupportTicketCreateDTOValidator : AbstractValidator<SupportTicketCreateDTO>
    {
        public SupportTicketCreateDTOValidator() 
        {
            RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");
        }
    }
}
