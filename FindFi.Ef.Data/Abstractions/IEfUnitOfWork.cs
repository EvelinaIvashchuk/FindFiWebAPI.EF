using System;
using System.Threading;
using System.Threading.Tasks;

namespace FindFi.Ef.Data.Abstractions;

public interface IEfUnitOfWork : IAsyncDisposable, IDisposable
{
    ICustomerRepository Customers { get; }
    IOrderRepository Orders { get; }
    IProductRepository Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}