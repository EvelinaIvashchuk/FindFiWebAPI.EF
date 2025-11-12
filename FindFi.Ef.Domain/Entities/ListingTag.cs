namespace FindFi.Ef.Domain.Entities;

// Join entity for many-to-many between Listing and Tag
public class ListingTag
{
    public long ListingId { get; set; }
    public long TagId { get; set; }

    public Listing Listing { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
