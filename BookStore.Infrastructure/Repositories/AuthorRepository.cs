using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repositories;

/// <summary>
/// Müəllif repository-si.
/// </summary>
public class AuthorRepository : Repository<Author>, IAuthorRepository
{
    public AuthorRepository(BookStoreDbContext context) : base(context)
    {
    }

    public async Task<Author?> GetWithBooksAsync(int id)
        => await DbSet
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<bool> ExistsByNameAsync(string fullName)
        => await DbSet.AnyAsync(a => a.FullName.ToLower() == fullName.ToLower());
}
