namespace BookStore.Application.DTOs.Customers;

/// <summary>
/// Müştəri cavab modeli.
/// </summary>
public class CustomerResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int OrderCount { get; set; }
}
