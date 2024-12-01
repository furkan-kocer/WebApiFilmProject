using FilmProject.DataAccess.DataTransferObjects.User;
using FluentValidation;
namespace FilmProject.Services.Validations.FluentValidation.UserRegisterValidation
{
    public class UserRequestDtoValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserRequestDtoValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email is required.")
                .NotNull().WithMessage("Email cannot be null.")
                .EmailAddress().WithMessage("Please enter valid email address.")
                .MaximumLength(100).WithMessage("Email field cannot exceed 100 characters.");
            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Your password cannot be empty.")
                .NotNull().WithMessage("Your password cannot be null.")
                .MaximumLength(100).WithMessage("Your password field cannot exceed 100 characters.")
                .MinimumLength(8).WithMessage("Your password length must include at least 8 characters.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
        }
    }
}
