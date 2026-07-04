using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Persistence;
using BookStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Infrastructure;

/// <summary>
/// Infrastructure qatının DI qeydiyyatı — appsettings.json-dakı "Database:Provider"
/// açarına görə PostgreSQL və ya SQL Server seçilir.
/// </summary>
public static class DependencyInjection
{
    private const string PostgreSqlProvider = "PostgreSQL";
    private const string SqlServerProvider = "SqlServer";

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var provider = configuration["Database:Provider"] ?? PostgreSqlProvider;

        services.AddDbContext<BookStoreDbContext>(options =>
        {
            if (string.Equals(provider, SqlServerProvider, StringComparison.OrdinalIgnoreCase))
            {
                options.UseSqlServer(configuration.GetConnectionString(SqlServerProvider));
            }
            else
            {
                options.UseNpgsql(configuration.GetConnectionString(PostgreSqlProvider));
            }
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
