using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FindFi.Ef.Domain.Entities;

namespace FindFi.Ef.Data.Abstractions;

public interface IOrderRepository : IAsyncRepository<Order>
{
    // Eager loading
    Task<Order?> GetWithItemsAsync(long orderId, CancellationToken cancellationToken = default);

    // Explicit loading
    Task LoadItemsAsync(Order order, CancellationToken cancellationToken = default);

    // LINQ queries
    Task<List<Order>> GetByStatusPagedAsync(byte status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<List<Order>> GetByCustomerInDateRangeAsync(long customerId, DateTime? from, DateTime? to, CancellationToken cancellationToken = default);
}