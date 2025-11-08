using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FindFi.Ef.Domain.Entities;

namespace FindFi.Ef.Data.Abstractions;

public interface IProductRepository : IAsyncRepository<Product>
{
    // Eager loading
    Task<Product?> GetWithOrderItemsAsync(long productId, CancellationToken cancellationToken = default);

    // Explicit loading
    Task LoadOrderItemsAsync(Product product, CancellationToken cancellationToken = default);

    // Lookups
    Task<Product?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    // LINQ many-to-many via join entity OrderItem
    Task<List<Product>> GetTopSellingProductsAsync(int topN, CancellationToken cancellationToken = default);
    Task<List<Order>> GetOrdersForProductAsync(long productId, CancellationToken cancellationToken = default);
}