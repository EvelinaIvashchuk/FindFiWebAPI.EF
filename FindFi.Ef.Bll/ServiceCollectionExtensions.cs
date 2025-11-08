using FindFi.Ef.Bll.Abstractions;
using FindFi.Ef.Bll.Mapping;
using FindFi.Ef.Bll.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FindFi.Ef.Bll;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEfBll(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ProductProfile>();
        }, typeof(ProductProfile).Assembly);

        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}
