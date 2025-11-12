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

        // Open-generic repository registration
        services.AddScoped(typeof(IAsyncRepository<>), typeof(GenericRepository<>));
        
        return services;
    }
}
