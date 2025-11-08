using FindFi.Ef.Data.Abstractions;
using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FindFi.Ef.Data.Repositories;

public class ProductRepository(AppDbContext db) : GenericRepository<Product>(db), IProductRepository
{
    // Eager loading of order items for a product
    public Task<Product?> GetWithOrderItemsAsync(long productId, CancellationToken cancellationToken = default)
    {
        return _db.Products
            .Include(p => p.OrderItems)
                .ThenInclude(oi => oi.Order)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);
    }

    // Explicit loading of order items
    public async Task LoadOrderItemsAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _db.Entry(product)
            .Collection(p => p.OrderItems)
            .LoadAsync(cancellationToken);

        // Optionally load Orders for each OrderItem explicitly
        foreach (var oi in product.OrderItems)
        {
            await _db.Entry(oi)
                .Reference(x => x.Order)
                .LoadAsync(cancellationToken);
        }
    }

    public Task<Product?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
    }

    // LINQ many-to-many via OrderItem join: top selling products by total quantity
    public async Task<List<Product>> GetTopSellingProductsAsync(int topN, CancellationToken cancellationToken = default)
    {
        if (topN <= 0) topN = 10;

        var query = from p in _db.Products.AsNoTracking()
                    join oi in _db.OrderItems.AsNoTracking() on p.Id equals oi.ProductId
                    group oi by new { p.Id, p.Name, p.Code, p.Price, p.IsActive, p.CreatedAt } into g
                    orderby g.Sum(x => x.Quantity) descending
                    select new { g.Key.Id };

        var topIds = await query.Take(topN).Select(x => x.Id).ToListAsync(cancellationToken);
        return await _db.Products.AsNoTracking()
            .Where(p => topIds.Contains(p.Id))
            .ToListAsync(cancellationToken);
    }

    // Orders that contain the given product (distinct)
    public Task<List<Order>> GetOrdersForProductAsync(long productId, CancellationToken cancellationToken = default)
    {
        var orders = from o in _db.Orders.AsNoTracking()
                     join oi in _db.OrderItems.AsNoTracking() on o.OrderId equals oi.OrderId
                     where oi.ProductId == productId
                     select o;
        return orders.Distinct().OrderByDescending(o => o.CreatedAt).ToListAsync(cancellationToken);
    }
}
