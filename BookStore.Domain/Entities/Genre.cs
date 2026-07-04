using BookStore.Domain.Common;

namespace BookStore.Domain.Entities;

/// <summary>
/// Janr entity-si. Bir janra bir neçə kitab aid ola bilər (1-N).
/// </summary>
public class Genre : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = new List<Book>();
}
