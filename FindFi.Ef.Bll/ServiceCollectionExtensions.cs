using AutoMapper;
using FindFi.Ef.Bll.Abstractions;
using FindFi.Ef.Bll.Mapping;
using FindFi.Ef.Bll.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FindFi.Ef.Bll;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEfBll(this IServiceCollection services)
    {
        // AutoMapper: scan current assembly for profiles
        services.AddAutoMapper(typeof(ApiMappingProfile).Assembly);

        // BLL services
        services.AddScoped<IListingService, ListingService>();
        services.AddScoped<ITagService, TagService>();

        return services;
    }
}
