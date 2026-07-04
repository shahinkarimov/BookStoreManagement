using BookStore.Domain.Common;

namespace BookStore.Domain.Entities;

/// <summary>
/// Kitab entity-si. Author (N-1), Genre (N-1) və OrderItem (1-N) ilə əlaqəlidir.
/// </summary>
public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
