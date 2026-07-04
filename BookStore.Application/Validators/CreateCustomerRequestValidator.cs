using BookStore.Application.DTOs.Customers;
using FluentValidation;

namespace BookStore.Application.Validators;

/// <summary>
/// Müştəri qeydiyyatı sorğusunun yoxlanışı — ad, düzgün email, telefon formatı.
/// </summary>
public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequestDto>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Müştərinin adı boş ola bilməz.")
            .MinimumLength(2).WithMessage("Müştərinin adı ən az 2 simvol olmalıdır.")
            .MaximumLength(150).WithMessage("Müştərinin adı 150 simvoldan uzun ola bilməz.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş ola bilməz.")
            .EmailAddress().WithMessage("Email formatı düzgün deyil.");

        RuleFor(x => x.Phone)
            .Matches(@"^\+?[0-9\s\-()]{7,20}$").WithMessage("Telefon nömrəsi düzgün formatda deyil.")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));
    }
}
