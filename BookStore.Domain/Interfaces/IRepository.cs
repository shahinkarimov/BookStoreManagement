using System.Linq.Expressions;
using BookStore.Domain.Common;

namespace BookStore.Domain.Interfaces;

/// <summary>
/// Generic repository müqaviləsi — bütün entity-lər üçün ortaq data-access əməliyyatları.
/// Implementasiya Infrastructure qatındadır (Dependency Inversion).
/// </summary>
public interface IRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
