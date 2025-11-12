namespace FindFi.Ef.Bll.DTOs;

using System;
using FindFi.Ef.Domain.Entities;

public class ListingQuery
{
    // Paging
    public int Page { get; set; } = 1; // starts from 1
    public int PageSize { get; set; } = 12; // default page size

    // Filtering
    public string? City { get; set; }
    public PropertyType? PropertyType { get; set; }
    public ListingType? ListingType { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string[]? Tags { get; set; }
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }

    // Sorting
    public string? SortBy { get; set; } // createdAt|price|city
    public string? SortDir { get; set; } // asc|desc

    public const int MaxPageSize = 100;

    public void Normalize()
    {
        if (Page < 1) Page = 1;
        if (PageSize <= 0) PageSize = 12;
        if (PageSize > MaxPageSize) PageSize = MaxPageSize;
        SortBy = string.IsNullOrWhiteSpace(SortBy) ? "createdAt" : SortBy.Trim();
        SortDir = string.IsNullOrWhiteSpace(SortDir) ? "desc" : SortDir.Trim().ToLowerInvariant();
        if (DateFrom.HasValue && DateTo.HasValue && DateFrom > DateTo)
        {
            // swap to keep range valid
            var tmp = DateFrom;
            DateFrom = DateTo;
            DateTo = tmp;
        }
    }
}
