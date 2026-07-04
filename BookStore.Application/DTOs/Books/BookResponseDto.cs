namespace BookStore.Application.DTOs.Books;

/// <summary>
/// Kitabın Presentation qatına ötürülən cavab modeli.
/// Domain entity heç vaxt birbaşa çıxarılmır.
/// </summary>
public class BookResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string GenreName { get; set; } = string.Empty;
}
