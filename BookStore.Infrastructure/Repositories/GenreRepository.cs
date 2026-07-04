using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repositories;

/// <summary>
/// Janr repository-si.
/// </summary>
public class GenreRepository : Repository<Genre>, IGenreRepository
{
    public GenreRepository(BookStoreDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByNameAsync(string name)
        => await DbSet.AnyAsync(g => g.Name.ToLower() == name.ToLower());
}
