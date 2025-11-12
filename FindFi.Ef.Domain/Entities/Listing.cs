namespace FindFi.Ef.Domain.Entities;

public class Listing
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
    public bool PetsAllowed { get; set; } = false;
    public string DefaultCurrency { get; set; } = "UAH";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Pricing? Pricing { get; set; }
    public List<Media> Media { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
    public List<Availability> Availabilities { get; set; } = new();
}
