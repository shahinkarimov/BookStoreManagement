using BookStore.Application.DTOs.Customers;

namespace BookStore.Application.Services.Interfaces;

/// <summary>
/// Müştəri əməliyyatlarının servis müqaviləsi.
/// </summary>
public interface ICustomerService
{
    Task<CustomerResponseDto> RegisterAsync(CreateCustomerRequestDto request);
    Task<IReadOnlyList<CustomerResponseDto>> GetAllAsync();
}
