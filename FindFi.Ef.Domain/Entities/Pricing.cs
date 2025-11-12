namespace FindFi.Ef.Domain.Entities;

public class Pricing
{
    public long ListingId { get; set; }
    public decimal? PricePerMonth { get; set; }
    public decimal? PricePerNight { get; set; }
    public string Currency { get; set; } = "UAH";
    public DateTime CreatedAt { get; set; }

    public Listing Listing { get; set; } = null!;
}
