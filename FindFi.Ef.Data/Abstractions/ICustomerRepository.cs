using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FindFi.Ef.Domain.Entities;

namespace FindFi.Ef.Data.Abstractions;

public interface ICustomerRepository : IAsyncRepository<Customer>
{
    Task<Customer?> GetWithAddressesAsync(long customerId, CancellationToken cancellationToken = default);
    Task LoadAddressesAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<Customer>> GetActiveAsync(CancellationToken cancellationToken = default);
}