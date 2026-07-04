using BookStore.Domain.Entities;

namespace BookStore.Domain.Interfaces;

/// <summary>
/// Kitaba xas sorğular — axtarış zamanı Author və Genre naviqasiyaları da yüklənir.
/// </summary>
public interface IBookRepository : IRepository<Book>
{
    Task<IReadOnlyList<Book>> GetAllWithDetailsAsync();
    Task<Book?> GetByIdWithDetailsAsync(int id);
    Task<IReadOnlyList<Book>> SearchAsync(string term);
    Task<IReadOnlyList<Book>> GetByGenreAsync(int genreId);
    Task<IReadOnlyList<Book>> GetByAuthorAsync(int authorId);
}
