using BookStore.Application.DTOs.Books;
using FluentValidation;

namespace BookStore.Application.Validators;

/// <summary>
/// Yeni kitab sorğusunun yoxlanışı: boş ad, mənfi qiymət, mənfi stok qadağandır.
/// </summary>
public class CreateBookRequestValidator : AbstractValidator<CreateBookRequestDto>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Kitabın adı boş ola bilməz.")
            .MaximumLength(200).WithMessage("Kitabın adı 200 simvoldan uzun ola bilməz.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Qiymət 0-dan böyük olmalıdır.")
            .LessThanOrEqualTo(100000).WithMessage("Qiymət real hədləri aşır.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stok mənfi ola bilməz.");

        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessage("Düzgün müəllif seçilməlidir.");

        RuleFor(x => x.GenreId)
            .GreaterThan(0).WithMessage("Düzgün janr seçilməlidir.");
    }
}
