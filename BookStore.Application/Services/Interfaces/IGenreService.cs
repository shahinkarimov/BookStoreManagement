using BookStore.Application.DTOs.Books;
using BookStore.Application.DTOs.Genres;

namespace BookStore.Application.Services.Interfaces;

/// <summary>
/// Janr əməliyyatlarının servis müqaviləsi.
/// </summary>
public interface IGenreService
{
    Task<GenreResponseDto> CreateAsync(CreateGenreRequestDto request);
    Task<IReadOnlyList<GenreResponseDto>> GetAllAsync();
    Task<IReadOnlyList<BookResponseDto>> GetBooksAsync(int genreId);
}
