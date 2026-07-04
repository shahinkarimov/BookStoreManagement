using BookStore.Domain.Common;

namespace BookStore.Domain.Entities;

/// <summary>
/// Müəllif entity-si. Bir müəllifin bir neçə kitabı ola bilər (1-N).
/// </summary>
public class Author : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string? Country { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}
