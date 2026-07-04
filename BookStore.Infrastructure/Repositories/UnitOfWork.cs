using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Persistence;

namespace BookStore.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementasiyası — bütün repository-lər eyni DbContext-i paylaşır,
/// beləliklə SaveChangesAsync bütün dəyişiklikləri tək tranzaksiyada yazır.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly BookStoreDbContext _context;

    public UnitOfWork(BookStoreDbContext context)
    {
        _context = context;
        Books = new BookRepository(context);
        Authors = new AuthorRepository(context);
        Genres = new GenreRepository(context);
        Customers = new CustomerRepository(context);
        Orders = new OrderRepository(context);
    }

    public IBookRepository Books { get; }
    public IAuthorRepository Authors { get; }
    public IGenreRepository Genres { get; }
    public ICustomerRepository Customers { get; }
    public IOrderRepository Orders { get; }

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
