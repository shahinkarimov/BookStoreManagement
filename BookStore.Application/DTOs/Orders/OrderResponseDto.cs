namespace BookStore.Application.DTOs.Orders;

/// <summary>
/// Sifariş cavab modeli — tarix, cəmi məbləğ və kitab sətirləri.
/// </summary>
public class OrderResponseDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public List<OrderItemResponseDto> Items { get; set; } = new();
}

/// <summary>
/// Sifariş sətrinin cavab modeli.
/// </summary>
public class OrderItemResponseDto
{
    public string BookTitle { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}
