namespace BookStore.Application.DTOs.Customers;

/// <summary>
/// Yeni müştəri qeydiyyatı üçün sorğu modeli.
/// </summary>
public class CreateCustomerRequestDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
}
