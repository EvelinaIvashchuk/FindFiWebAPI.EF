using System;
using System.Threading;
using System.Threading.Tasks;

namespace FindFi.Ef.Data.Abstractions;

public interface IEfUnitOfWork : IAsyncDisposable, IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}