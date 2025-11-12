using System;
using System.Collections.Generic;
using FindFi.Ef.Domain.Entities;

namespace FindFi.Ef.Bll.DTOs;

public class ListingDto
{
    public long Id { get; set; }
    public long HostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public PropertyType PropertyType { get; set; }
    public ListingType ListingType { get; set; }
    public string City { get; set; } = string.Empty;
    public string? AddressLine { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public byte? Rooms { get; set; }
    public byte? Bedrooms { get; set; }
    public byte? Bathrooms { get; set; }
    public decimal? AreaSqM { get; set; }
    public bool PetsAllowed { get; set; }
    public string DefaultCurrency { get; set; } = "UAH";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public PricingDto? Pricing { get; set; }
    public List<MediaDto> Media { get; set; } = new();
    public List<TagDto> Tags { get; set; } = new();
    public List<AvailabilityDto> Availabilities { get; set; } = new();
}

public class MediaDto
{
    public long Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsCover { get; set; }
    public int SortOrder { get; set; }
}

public class PricingDto
{
    public decimal? PricePerMonth { get; set; }
    public decimal? PricePerNight { get; set; }
    public string Currency { get; set; } = "UAH";
}

public class TagDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
}

public class AvailabilityDto
{
    public long Id { get; set; }
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public bool IsAvailable { get; set; }
}
