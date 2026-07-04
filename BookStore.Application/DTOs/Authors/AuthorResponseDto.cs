namespace BookStore.Application.DTOs.Authors;

/// <summary>
/// Müəllif cavab modeli — kitab sayı ilə birlikdə.
/// </summary>
public class AuthorResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Country { get; set; }
    public int BookCount { get; set; }
}
