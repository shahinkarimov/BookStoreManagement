using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repositories;

/// <summary>
/// Kitab repository-si — Author/Genre ilə birgə yükləmə və axtarış sorğuları.
/// </summary>
public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(BookStoreDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Book>> GetAllWithDetailsAsync()
        => await DbSet.AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .OrderBy(b => b.Title)
            .ToListAsync();

    public async Task<Book?> GetByIdWithDetailsAsync(int id)
        => await DbSet
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<IReadOnlyList<Book>> SearchAsync(string term)
    {
        var pattern = $"%{term}%";
        return await DbSet.AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Where(b =>
                EF.Functions.Like(b.Title, pattern) ||
                EF.Functions.Like(b.Author.FullName, pattern) ||
                EF.Functions.Like(b.Genre.Name, pattern))
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Book>> GetByGenreAsync(int genreId)
        => await DbSet.AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Where(b => b.GenreId == genreId)
            .OrderBy(b => b.Title)
            .ToListAsync();

    public async Task<IReadOnlyList<Book>> GetByAuthorAsync(int authorId)
        => await DbSet.AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Where(b => b.AuthorId == authorId)
            .OrderBy(b => b.Title)
            .ToListAsync();
}
