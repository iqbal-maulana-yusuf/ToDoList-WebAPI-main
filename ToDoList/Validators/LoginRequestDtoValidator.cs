using FluentValidation;
using ToDoList.Dtos.LoginDto;

namespace ToDoList.Validators
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email tidak boleh kosong.")
                .EmailAddress().WithMessage("Format email tidak valid.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password tidak boleh kosong.")
                .MinimumLength(6).WithMessage("Password harus memiliki minimal 6 karakter.");
        }
    }
}
