using BookStore.Application.DTOs.Orders;

namespace BookStore.Application.Services.Interfaces;

/// <summary>
/// Sifariş əməliyyatlarının servis müqaviləsi.
/// </summary>
public interface IOrderService
{
    Task<OrderResponseDto> CreateAsync(CreateOrderRequestDto request);
    Task<IReadOnlyList<OrderResponseDto>> GetByCustomerAsync(int customerId);
    Task<IReadOnlyList<OrderResponseDto>> GetAllAsync();
    Task<OrderResponseDto?> GetByIdAsync(int id);
}
