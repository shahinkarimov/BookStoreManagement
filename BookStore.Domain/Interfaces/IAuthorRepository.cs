using BookStore.Domain.Entities;

namespace BookStore.Domain.Interfaces;

/// <summary>
/// Müəllifə xas sorğular — kitabları ilə birlikdə yükləmə.
/// </summary>
public interface IAuthorRepository : IRepository<Author>
{
    Task<Author?> GetWithBooksAsync(int id);
    Task<bool> ExistsByNameAsync(string fullName);
}
