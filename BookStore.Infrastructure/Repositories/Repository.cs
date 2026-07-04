using System.Linq.Expressions;
using BookStore.Domain.Common;
using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repositories;

/// <summary>
/// Generic repository — yalnız data-access; biznes məntiqi burada yoxdur.
/// </summary>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly BookStoreDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(BookStoreDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
        => await DbSet.AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdAsync(int id)
        => await DbSet.FindAsync(id);

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await DbSet.AsNoTracking().Where(predicate).ToListAsync();

    public async Task AddAsync(T entity)
        => await DbSet.AddAsync(entity);

    public void Update(T entity)
        => DbSet.Update(entity);

    public void Delete(T entity)
        => DbSet.Remove(entity);
}
