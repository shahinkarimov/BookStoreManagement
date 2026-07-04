using BookStore.Domain.Entities;

namespace BookStore.Domain.Interfaces;

/// <summary>
/// Müştəriyə xas sorğular — email unikallığı yoxlanışı üçün.
/// </summary>
public interface ICustomerRepository : IRepository<Customer>
{
    Task<bool> ExistsByEmailAsync(string email);
}
