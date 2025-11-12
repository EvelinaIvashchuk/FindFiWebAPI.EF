namespace FindFi.Ef.Domain.Entities;

public class Media
{
    public long Id { get; set; }
    public long ListingId { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsCover { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }

    public Listing Listing { get; set; } = null!;
}
