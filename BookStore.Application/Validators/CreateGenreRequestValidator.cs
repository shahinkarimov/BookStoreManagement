using BookStore.Application.DTOs.Genres;
using FluentValidation;

namespace BookStore.Application.Validators;

/// <summary>
/// Yeni janr sorğusunun yoxlanışı.
/// </summary>
public class CreateGenreRequestValidator : AbstractValidator<CreateGenreRequestDto>
{
    public CreateGenreRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Janr adı boş ola bilməz.")
            .MinimumLength(2).WithMessage("Janr adı ən az 2 simvol olmalıdır.")
            .MaximumLength(100).WithMessage("Janr adı 100 simvoldan uzun ola bilməz.");
    }
}
