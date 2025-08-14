using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models;
using FluentValidation;
using ToDoList.Dtos.RegisterDto;

namespace ToDoList.Validators
{

    public class RegisterValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username wajib diisi.")
                .MinimumLength(3).WithMessage("Username minimal 3 karakter.")
                .MaximumLength(20).WithMessage("Username maksimal 20 karakter.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email wajib diisi.")
                .EmailAddress().WithMessage("Format email tidak valid.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password wajib diisi.")
                .MinimumLength(6).WithMessage("Password minimal 6 karakter.")
                .Matches("[A-Z]").WithMessage("Password harus mengandung huruf kapital.")
                .Matches("[0-9]").WithMessage("Password harus mengandung angka.");
        }
    }
}
