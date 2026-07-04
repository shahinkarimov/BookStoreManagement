using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookStore.Infrastructure.Persistence;

/// <summary>
/// Design-time factory — "dotnet ef migrations add" əmri işləyəndə
/// Presentation-ın DI konteynerini qaldırmadan DbContext yaradır.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookStoreDbContext>
{
    public BookStoreDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookStoreDbContext>();

        // Migration generasiyası üçün default provider PostgreSQL-dir.
        // SQL Server üçün ayrıca migration lazım olsa, connection string-i dəyişin.
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=bookstore;Username=postgres;Password=postgres");

        return new BookStoreDbContext(optionsBuilder.Options);
    }
}
