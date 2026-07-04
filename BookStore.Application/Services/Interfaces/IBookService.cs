using BookStore.Application.DTOs.Books;

namespace BookStore.Application.Services.Interfaces;

/// <summary>
/// Kitab əməliyyatlarının servis müqaviləsi — Presentation yalnız bununla danışır.
/// </summary>
public interface IBookService
{
    Task<BookResponseDto> CreateAsync(CreateBookRequestDto request);
    Task<IReadOnlyList<BookResponseDto>> GetAllAsync();
    Task<BookResponseDto?> GetByIdAsync(int id);
    Task<IReadOnlyList<BookResponseDto>> SearchAsync(string term);
    Task<IReadOnlyList<BookResponseDto>> GetByGenreAsync(int genreId);
    Task<IReadOnlyList<BookResponseDto>> GetByAuthorAsync(int authorId);
    Task<BookResponseDto> UpdateAsync(UpdateBookRequestDto request);
    Task DeleteAsync(int id);
}
