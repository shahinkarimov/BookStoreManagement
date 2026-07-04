using BookStore.Application.DTOs.Authors;
using BookStore.Application.DTOs.Books;

namespace BookStore.Application.Services.Interfaces;

/// <summary>
/// Müəllif əməliyyatlarının servis müqaviləsi.
/// </summary>
public interface IAuthorService
{
    Task<AuthorResponseDto> CreateAsync(CreateAuthorRequestDto request);
    Task<IReadOnlyList<AuthorResponseDto>> GetAllAsync();
    Task<IReadOnlyList<BookResponseDto>> GetBooksAsync(int authorId);
}
