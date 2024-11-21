using FilmProject.DataAccess.DataTransferObjects.Film;
using FluentValidation;

namespace FilmProject.Services.Validations.FluentValidation.FilmValidation
{
    public class FilmRequestDtoValidator : AbstractValidator<FilmRequest>
    {
        public FilmRequestDtoValidator()
        {
            RuleFor(r => r.FilmName).NotEmpty().WithMessage("FilmName is required.")
                .NotNull().WithMessage("FilmName cannot be null")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");
            RuleFor(r => r.Price)
                .Must(r=>r >= 0).WithMessage("Price cannot be negative value");
        }
    }
}
