using FindFi.Ef.Data.Abstractions;
using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FindFi.Ef.Data.Repositories;

public class OrderRepository(AppDbContext db) : GenericRepository<Order>(db), IOrderRepository
{
    // Eager loading: include items and related products
    public async Task<Order?> GetWithItemsAsync(long orderId, CancellationToken cancellationToken = default)
    {
        return await _db.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Include(o => o.Details)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.OrderId == orderId, cancellationToken);
    }

    // Explicit loading for items (and optionally product per item)
    public async Task LoadItemsAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _db.Entry(order)
            .Collection(o => o.Items)
            .LoadAsync(cancellationToken);

        // Optionally explicitly load Product for each item
        foreach (var item in order.Items)
        {
            await _db.Entry(item)
                .Reference(i => i.Product)
                .LoadAsync(cancellationToken);
        }
    }

    public Task<List<Order>> GetByStatusPagedAsync(byte status, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize <= 0) pageSize = 10;
        return _db.Orders
            .AsNoTracking()
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Order>> GetByCustomerInDateRangeAsync(long customerId, DateTime? from, DateTime? to, CancellationToken cancellationToken = default)
    {
        var query = _db.Orders.AsNoTracking().Where(o => o.CustomerId == customerId);
        if (from.HasValue)
        {
            query = query.Where(o => o.PlacedAt != null && o.PlacedAt >= from.Value);
        }
        if (to.HasValue)
        {
            query = query.Where(o => o.PlacedAt != null && o.PlacedAt <= to.Value);
        }
        return query.OrderByDescending(o => o.PlacedAt).ToListAsync(cancellationToken);
    }
}
