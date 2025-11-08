using FindFi.Ef.Data.Abstractions;
using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FindFi.Ef.Data.Repositories;

public class CustomerRepository(AppDbContext db) : GenericRepository<Customer>(db), ICustomerRepository
{
    public async Task<Customer?> GetWithAddressesAsync(long customerId, CancellationToken cancellationToken = default)
    {
        return await _db.Customers
            .Include(c => c.Addresses)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
    }

    public async Task LoadAddressesAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _db.Entry(customer)
            .Collection(c => c.Addresses)
            .LoadAsync(cancellationToken);
    }

    public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    public Task<List<Customer>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return _db.Customers.AsNoTracking().Where(c => c.IsActive).OrderByDescending(c => c.CreatedAt).ToListAsync(cancellationToken);
    }
}