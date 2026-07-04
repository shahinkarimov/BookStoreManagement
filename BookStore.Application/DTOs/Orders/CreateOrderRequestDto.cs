namespace BookStore.Application.DTOs.Orders;

/// <summary>
/// Yeni sifariş yaratmaq üçün sorğu modeli — müştəri + kitab sətirləri.
/// </summary>
public class CreateOrderRequestDto
{
    public int CustomerId { get; set; }
    public List<OrderItemRequestDto> Items { get; set; } = new();
}

/// <summary>
/// Sifarişin bir sətri: hansı kitabdan neçə ədəd.
/// </summary>
public class OrderItemRequestDto
{
    public int BookId { get; set; }
    public int Quantity { get; set; }
}
