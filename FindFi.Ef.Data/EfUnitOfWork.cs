using System;
using System.Threading;
using System.Threading.Tasks;
using FindFi.Ef.Data.Abstractions;
using FindFi.Ef.Data.Repositories;

namespace FindFi.Ef.Data;

public sealed class EfUnitOfWork : IEfUnitOfWork
{
    private readonly AppDbContext _db;
    private ICustomerRepository? _customers;
    private IOrderRepository? _orders;
    private IProductRepository? _products;
    private bool _disposed;

    public EfUnitOfWork(AppDbContext db)
    {
        _db = db;
    }

    public ICustomerRepository Customers => _customers ??= new CustomerRepository(_db);
    public IOrderRepository Orders => _orders ??= new OrderRepository(_db);
    public IProductRepository Products => _products ??= new ProductRepository(_db);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _db.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _db.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;
        await _db.DisposeAsync();
    }
}
