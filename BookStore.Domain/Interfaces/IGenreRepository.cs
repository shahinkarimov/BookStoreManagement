using BookStore.Domain.Entities;

namespace BookStore.Domain.Interfaces;

/// <summary>
/// Janra xas sorğular.
/// </summary>
public interface IGenreRepository : IRepository<Genre>
{
    Task<bool> ExistsByNameAsync(string name);
}
