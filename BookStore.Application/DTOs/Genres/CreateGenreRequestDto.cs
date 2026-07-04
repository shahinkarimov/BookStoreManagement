namespace BookStore.Application.DTOs.Genres;

/// <summary>
/// Yeni janr yaratmaq üçün sorğu modeli.
/// </summary>
public class CreateGenreRequestDto
{
    public string Name { get; set; } = string.Empty;
}
