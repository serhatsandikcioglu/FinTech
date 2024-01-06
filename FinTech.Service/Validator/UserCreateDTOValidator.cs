using FinTech.Core.DTOs.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Validator
{
    public class UserCreateDTOValidator : AbstractValidator<UserCreateDTO>
    {
        public UserCreateDTOValidator() 
        {
            RuleFor(x => x.Name)
          .NotNull().WithMessage("Name cannot be empty.")
          .MinimumLength(5);

            RuleFor(x => x.Surname)
                .NotNull().WithMessage("Surname cannot be empty.");

            RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Password cannot be empty.")
           .MinimumLength(6).WithMessage("Password must be at least 8 characters long.")
           .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
           .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
           .Matches(@"\d").WithMessage("Password must contain at least one digit.")
           .Matches(@"[!@#$%^&*(),.?""':{}|<>]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.IdentityNumber)
                .NotEmpty().WithMessage("IdentityNumber cannot be empty.")
                .Length(11).WithMessage("IdentityNumber must be 11 characters long.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address cannot be empty.");

            RuleFor(x => x.Mail)
                .NotEmpty().WithMessage("Mail cannot be empty.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber cannot be empty.")
                .Length(10).WithMessage("PhoneNumber must be 10 digits.");
        }
    }
}
