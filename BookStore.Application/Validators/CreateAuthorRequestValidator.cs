using BookStore.Application.DTOs.Authors;
using FluentValidation;

namespace BookStore.Application.Validators;

/// <summary>
/// Yeni müəllif sorğusunun yoxlanışı.
/// </summary>
public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorRequestDto>
{
    public CreateAuthorRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Müəllifin adı boş ola bilməz.")
            .MinimumLength(2).WithMessage("Müəllifin adı ən az 2 simvol olmalıdır.")
            .MaximumLength(150).WithMessage("Müəllifin adı 150 simvoldan uzun ola bilməz.");

        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("Ölkə adı 100 simvoldan uzun ola bilməz.")
            .When(x => !string.IsNullOrWhiteSpace(x.Country));
    }
}
