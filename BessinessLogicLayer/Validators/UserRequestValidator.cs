using FluentValidation;
using ModelLayer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BessinessLogicLayer.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequestDto>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MinimumLength(2).WithMessage("First Name must be at least 2 characters")
                .MaximumLength(100).WithMessage("First Name cannot exceed 100 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MinimumLength(2).WithMessage("Last Name must be at least 2 characters")
                .MaximumLength(100).WithMessage("Last Name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(255).WithMessage("Password cannot exceed 255 characters");
        }
    }
}
