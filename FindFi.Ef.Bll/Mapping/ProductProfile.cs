using AutoMapper;
using FindFi.Ef.Bll.DTOs;
using FindFi.Ef.Domain.Entities;

namespace FindFi.Ef.Bll.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id));

        CreateMap<CreateProductDto, Product>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore());

        CreateMap<UpdateProductDto, Product>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore());
    }
}
