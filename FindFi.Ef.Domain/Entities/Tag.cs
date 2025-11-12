namespace FindFi.Ef.Domain.Entities;

public class Tag
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;

    public List<Listing> Listings { get; set; } = new();
}
