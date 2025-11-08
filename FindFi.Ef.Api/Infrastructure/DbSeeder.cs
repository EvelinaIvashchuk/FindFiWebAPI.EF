using FindFi.Ef.Data;
using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FindFi.Ef.Api.Infrastructure;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        // Ensure parents first, then children
        if (!await db.Customers.AnyAsync(cancellationToken))
        {
            var customer = new Customer
            {
                Email = "john.doe@example.com",
                FullName = "John Doe",
                Phone = "+380501112233",
                Role = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            await db.Customers.AddAsync(customer, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            await db.CustomerAddresses.AddAsync(new CustomerAddress
            {
                CustomerId = customer.CustomerId,
                Line1 = "Main St 1",
                City = "Kyiv",
                Country = "Ukraine",
                IsPrimary = true,
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);
        }

        if (!await db.Products.AnyAsync(cancellationToken))
        {
            await db.Products.AddRangeAsync(new[]
            {
                new Product { Code = "APT-1", Name = "Apartment Center", Description = "Cozy apt.", Price = 500m, IsActive = true, CreatedAt = DateTime.UtcNow },
                new Product { Code = "HSE-1", Name = "House Suburb", Description = "Family house.", Price = 1200m, IsActive = true, CreatedAt = DateTime.UtcNow },
                new Product { Code = "RM-1", Name = "Room Budget", Description = "Budget room.", Price = 250m, IsActive = true, CreatedAt = DateTime.UtcNow }
            }, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}
