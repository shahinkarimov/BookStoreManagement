using BookStore.Application.DTOs.Books;
using FluentValidation;

namespace BookStore.Application.Validators;

/// <summary>
/// Kitab redaktə sorğusunun yoxlanışı.
/// </summary>
public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequestDto>
{
    public UpdateBookRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Düzgün kitab ID-si göstərilməlidir.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Kitabın adı boş ola bilməz.")
            .MaximumLength(200).WithMessage("Kitabın adı 200 simvoldan uzun ola bilməz.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Qiymət 0-dan böyük olmalıdır.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stok mənfi ola bilməz.");
    }
}
