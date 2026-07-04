using BookStore.Domain.Common;

namespace BookStore.Domain.Entities;

/// <summary>
/// Sifariş entity-si. Customer (N-1) və OrderItem (1-N) ilə əlaqəlidir.
/// Kitablarla N-N əlaqə OrderItem cədvəli üzərindən qurulur.
/// </summary>
public class Order : BaseEntity
{
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
