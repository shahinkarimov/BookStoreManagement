using BookStore.Domain.Common;

namespace BookStore.Domain.Entities;

/// <summary>
/// Müştəri entity-si. Bir müştərinin bir neçə sifarişi ola bilər (1-N).
/// </summary>
public class Customer : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
