namespace BookStore.Application.DTOs.Books;

/// <summary>
/// Kitab redaktəsi üçün sorğu modeli (ad, qiymət, stok).
/// </summary>
public class UpdateBookRequestDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
