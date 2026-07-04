using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repositories;

/// <summary>
/// Sifariş repository-si — müştəri və kitab detalları ilə birgə yükləmə.
/// </summary>
public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(BookStoreDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Order>> GetByCustomerAsync(int customerId)
        => await DbSet.AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(i => i.Book)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

    public async Task<Order?> GetWithDetailsAsync(int id)
        => await DbSet.AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(i => i.Book)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IReadOnlyList<Order>> GetAllWithDetailsAsync()
        => await DbSet.AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(i => i.Book)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
}
