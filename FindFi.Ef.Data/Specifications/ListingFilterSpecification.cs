using System;
using System.Linq;
using FindFi.Ef.Data.Specifications;
using FindFi.Ef.Domain.Entities;

namespace FindFi.Ef.Data.Specifications;

public sealed class ListingFilterSpecification : BaseSpecification<Listing>
{
    public ListingFilterSpecification(
        string? city,
        PropertyType? propertyType,
        ListingType? listingType,
        decimal? minPrice,
        decimal? maxPrice,
        string[]? tags,
        DateOnly? dateFrom,
        DateOnly? dateTo,
        string sortBy,
        string sortDir,
        int skip,
        int take)
    {
        // Normalize inputs
        var normalizedCity = string.IsNullOrWhiteSpace(city) ? null : city.Trim();
        var tagCodes = tags is { Length: > 0 }
            ? tags.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToArray()
            : Array.Empty<string>();

        Criteria = l =>
            (normalizedCity == null || l.City == normalizedCity) &&
            (!propertyType.HasValue || l.PropertyType == propertyType.Value) &&
            (!listingType.HasValue || l.ListingType == listingType.Value) &&
            // Price filter: if listingType is provided, filter by its corresponding price column.
            (!minPrice.HasValue ||
                (!listingType.HasValue && ((l.Pricing != null && ((l.Pricing.PricePerMonth ?? decimal.MaxValue) >= minPrice.Value) || (l.Pricing.PricePerNight ?? decimal.MaxValue) >= minPrice.Value))) ||
                (listingType == ListingType.LongTerm && l.Pricing != null && (l.Pricing.PricePerMonth ?? decimal.MaxValue) >= minPrice.Value) ||
                (listingType == ListingType.ShortTerm && l.Pricing != null && (l.Pricing.PricePerNight ?? decimal.MaxValue) >= minPrice.Value)
            ) &&
            (!maxPrice.HasValue ||
                (!listingType.HasValue && ((l.Pricing != null && ((l.Pricing.PricePerMonth ?? decimal.MinValue) <= maxPrice.Value) || (l.Pricing.PricePerNight ?? decimal.MinValue) <= maxPrice.Value))) ||
                (listingType == ListingType.LongTerm && l.Pricing != null && (l.Pricing.PricePerMonth ?? decimal.MinValue) <= maxPrice.Value) ||
                (listingType == ListingType.ShortTerm && l.Pricing != null && (l.Pricing.PricePerNight ?? decimal.MinValue) <= maxPrice.Value)
            ) &&
            (tagCodes.Length == 0 || l.Tags.Any(t => tagCodes.Contains(t.Code))) &&
            (
                (!dateFrom.HasValue && !dateTo.HasValue) ||
                (dateFrom.HasValue && !dateTo.HasValue && l.Availabilities.Any(a => a.IsAvailable && a.DateFrom <= dateFrom.Value && a.DateTo >= dateFrom.Value)) ||
                (!dateFrom.HasValue && dateTo.HasValue && l.Availabilities.Any(a => a.IsAvailable && a.DateFrom <= dateTo.Value && a.DateTo >= dateTo.Value)) ||
                (dateFrom.HasValue && dateTo.HasValue && l.Availabilities.Any(a => a.IsAvailable && a.DateFrom <= dateFrom.Value && a.DateTo >= dateTo.Value))
            );

        // Includes for list endpoint
        AddInclude(l => l.Pricing);
        AddInclude(l => l.Media);
        AddInclude(l => l.Tags);

        // Sorting
        var dirDesc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
        switch (sortBy?.ToLowerInvariant())
        {
            case "price":
                if (dirDesc)
                {
                    ApplyOrderByDescending(l => l.ListingType == ListingType.LongTerm
                        ? (object?)l.Pricing!.PricePerMonth
                        : l.Pricing!.PricePerNight!);
                }
                else
                {
                    ApplyOrderBy(l => l.ListingType == ListingType.LongTerm
                        ? (object?)l.Pricing!.PricePerMonth
                        : l.Pricing!.PricePerNight!);
                }
                break;
            case "city":
                if (dirDesc) ApplyOrderByDescending(l => l.City);
                else ApplyOrderBy(l => l.City);
                break;
            case "createdat":
            default:
                if (dirDesc) ApplyOrderByDescending(l => l.CreatedAt);
                else ApplyOrderBy(l => l.CreatedAt);
                break;
        }

        // Paging
        if (take > 0)
        {
            ApplyPaging(skip, take);
        }
    }
}
