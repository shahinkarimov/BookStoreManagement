using BookStore.Domain.Entities;

namespace BookStore.Domain.Interfaces;

/// <summary>
/// Sifarişə xas sorğular — detallar (müştəri, kitablar) ilə birlikdə yükləmə.
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
    Task<IReadOnlyList<Order>> GetByCustomerAsync(int customerId);
    Task<Order?> GetWithDetailsAsync(int id);
    Task<IReadOnlyList<Order>> GetAllWithDetailsAsync();
}
