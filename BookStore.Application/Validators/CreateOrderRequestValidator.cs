using BookStore.Application.DTOs.Orders;
using FluentValidation;

namespace BookStore.Application.Validators;

/// <summary>
/// Sifariş sorğusunun yoxlanışı — müştəri seçimi, ən az 1 sətir, müsbət say.
/// </summary>
public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequestDto>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("Düzgün müştəri seçilməlidir.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Sifarişdə ən az bir kitab olmalıdır.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.BookId)
                .GreaterThan(0).WithMessage("Düzgün kitab seçilməlidir.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Say 0-dan böyük olmalıdır.")
                .LessThanOrEqualTo(1000).WithMessage("Bir sifarişdə maksimum 1000 ədəd ola bilər.");
        });
    }
}
