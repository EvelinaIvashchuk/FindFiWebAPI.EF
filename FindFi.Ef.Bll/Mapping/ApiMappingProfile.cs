using AutoMapper;
using FindFi.Ef.Bll.DTOs;
using FindFi.Ef.Domain.Entities;
using System.Linq;

namespace FindFi.Ef.Bll.Mapping;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<Media, MediaDto>();
        CreateMap<Tag, TagDto>();
        CreateMap<Pricing, PricingDto>();
        CreateMap<Availability, AvailabilityDto>();

        CreateMap<Listing, ListingDto>()
            .ForMember(d => d.Media, opt => opt.MapFrom(s => s.Media.OrderBy(m => m.SortOrder)))
            .ForMember(d => d.Pricing, opt => opt.MapFrom(s => s.Pricing))
            .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags))
            .ForMember(d => d.Availabilities, opt => opt.MapFrom(s => s.Availabilities));
    }
}
