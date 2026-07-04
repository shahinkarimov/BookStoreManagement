namespace BookStore.Application.DTOs.Books;

/// <summary>
/// Yeni kitab yaratmaq üçün sorğu modeli.
/// </summary>
public class CreateBookRequestDto
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int AuthorId { get; set; }
    public int GenreId { get; set; }
}
