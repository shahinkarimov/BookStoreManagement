namespace BookStore.Application.DTOs.Genres;

/// <summary>
/// Janr cavab modeli — kitab sayı ilə birlikdə.
/// </summary>
public class GenreResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BookCount { get; set; }
}
