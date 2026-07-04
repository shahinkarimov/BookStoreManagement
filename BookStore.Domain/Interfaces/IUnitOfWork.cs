namespace BookStore.Domain.Interfaces;

/// <summary>
/// Unit of Work — bir neçə repository əməliyyatını tək tranzaksiyada birləşdirir.
/// SaveChangesAsync çağırılana qədər heç nə bazaya yazılmır.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IBookRepository Books { get; }
    IAuthorRepository Authors { get; }
    IGenreRepository Genres { get; }
    ICustomerRepository Customers { get; }
    IOrderRepository Orders { get; }

    Task<int> SaveChangesAsync();
}
