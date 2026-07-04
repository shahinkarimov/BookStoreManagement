using BookStore.Domain.Common;

namespace BookStore.Domain.Entities;

/// <summary>
/// Order ↔ Book arasında N-N əlaqəni quran aralıq entity.
/// UnitPrice sifariş anındakı qiyməti saxlayır (tarixi qiymət dəyişsə belə).
/// </summary>
public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
