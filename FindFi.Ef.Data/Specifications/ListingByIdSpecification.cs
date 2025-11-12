using FindFi.Ef.Data.Specifications;
using FindFi.Ef.Domain.Entities;

namespace FindFi.Ef.Data.Specifications;

public sealed class ListingByIdSpecification : BaseSpecification<Listing>
{
    public ListingByIdSpecification(long id)
    {
        Criteria = l => l.Id == id;
        AddInclude(l => l.Pricing);
        AddInclude(l => l.Media);
        AddInclude(l => l.Tags);
        AddInclude(l => l.Availabilities);
    }
}
