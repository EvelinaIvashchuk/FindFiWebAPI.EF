namespace FindFi.Ef.Domain.Entities;

public class Availability
{
    public long Id { get; set; }
    public long ListingId { get; set; }
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Listing Listing { get; set; } = null!;
}
