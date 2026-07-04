using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repositories;

/// <summary>
/// Müştəri repository-si.
/// </summary>
public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(BookStoreDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByEmailAsync(string email)
        => await DbSet.AnyAsync(c => c.Email == email);
}
