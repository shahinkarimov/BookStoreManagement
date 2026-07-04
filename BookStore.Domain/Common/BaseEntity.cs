namespace BookStore.Domain.Common;

/// <summary>
/// Bütün entity-lərin ortaq bazası — Id və yaradılma tarixi.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
