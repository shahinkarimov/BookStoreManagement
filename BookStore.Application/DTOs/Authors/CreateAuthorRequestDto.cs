namespace BookStore.Application.DTOs.Authors;

/// <summary>
/// Yeni müəllif yaratmaq üçün sorğu modeli.
/// </summary>
public class CreateAuthorRequestDto
{
    public string FullName { get; set; } = string.Empty;
    public string? Country { get; set; }
}
