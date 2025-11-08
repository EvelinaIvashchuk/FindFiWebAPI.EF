using FindFi.Ef.Data.Abstractions;
using FindFi.Ef.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FindFi.Ef.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEfDal(this IServiceCollection services)
    {
        // DbContext should be registered by the caller (API) with proper connection string and provider
        services.AddScoped<IEfUnitOfWork, EfUnitOfWork>();

        // Optional direct repos registration (resolve from UoW)
        services.AddScoped<ICustomerRepository>(sp => sp.GetRequiredService<IEfUnitOfWork>().Customers);
        services.AddScoped<IOrderRepository>(sp => sp.GetRequiredService<IEfUnitOfWork>().Orders);
        services.AddScoped<IProductRepository>(sp => sp.GetRequiredService<IEfUnitOfWork>().Products);

        return services;
    }
}
