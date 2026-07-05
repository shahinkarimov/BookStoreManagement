using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookStore.Infrastructure.Persistence;

/// <summary>
/// İlk işə salınmada bazanı yaradır (migration tətbiq edir) və nümunə data ilə doldurur.
/// </summary>
public static class DbInitializer
{
    public static async Task InitializeAsync(BookStoreDbContext context)
    {
        // Migration-lar PostgreSQL üçün generasiya olunub ("timestamp with time zone" və s.),
        // ona görə yalnız Npgsql provider-də tətbiq edilir. SQL Server-də schema
        // birbaşa modeldən yaradılır — EnsureCreated provider-ə uyğun DDL çıxarır.
        var isNpgsql = context.Database.ProviderName?.Contains("Npgsql") == true;

        if (isNpgsql && (await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }
        else
        {
            // EnsureCreated boş (cədvəlsiz) mövcud bazada heç nə etmir —
            // yarımçıq qalmış baza üçün cədvəlləri ayrıca yaradırıq.
            var creator = context.Database.GetService<IRelationalDatabaseCreator>();
            if (!await creator.ExistsAsync())
                await creator.CreateAsync();
            if (!await creator.HasTablesAsync())
                await creator.CreateTablesAsync();
        }

        if (await context.Books.AnyAsync())
            return; // Data artıq mövcuddur

        var genres = new List<Genre>
        {
            new() { Name = "Roman" },
            new() { Name = "Fantastika" },
            new() { Name = "Detektiv" },
            new() { Name = "Tarix" },
            new() { Name = "Proqramlaşdırma" }
        };

        var authors = new List<Author>
        {
            new() { FullName = "Çingiz Abdullayev", Country = "Azərbaycan" },
            new() { FullName = "İsa Hüseynov", Country = "Azərbaycan" },
            new() { FullName = "George Orwell", Country = "Böyük Britaniya" },
            new() { FullName = "Frank Herbert", Country = "ABŞ" },
            new() { FullName = "Robert C. Martin", Country = "ABŞ" }
        };

        await context.Genres.AddRangeAsync(genres);
        await context.Authors.AddRangeAsync(authors);
        await context.SaveChangesAsync();

        var books = new List<Book>
        {
            new() { Title = "Mavi mələklər", Price = 12.50m, Stock = 25, Author = authors[0], Genre = genres[2] },
            new() { Title = "İdeal", Price = 14.00m, Stock = 18, Author = authors[1], Genre = genres[0] },
            new() { Title = "1984", Price = 15.90m, Stock = 40, Author = authors[2], Genre = genres[0] },
            new() { Title = "Heyvanıstan", Price = 11.30m, Stock = 30, Author = authors[2], Genre = genres[0] },
            new() { Title = "Dune", Price = 22.75m, Stock = 15, Author = authors[3], Genre = genres[1] },
            new() { Title = "Clean Code", Price = 45.00m, Stock = 10, Author = authors[4], Genre = genres[4] },
            new() { Title = "Clean Architecture", Price = 48.50m, Stock = 8, Author = authors[4], Genre = genres[4] }
        };

        var customers = new List<Customer>
        {
            new() { FullName = "Şahin Kərimov", Email = "sahin@example.com", Phone = "+994501112233" },
            new() { FullName = "Aysel Məmmədova", Email = "aysel@example.com", Phone = "+994551234567" }
        };

        await context.Books.AddRangeAsync(books);
        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();
    }
}
