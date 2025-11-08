namespace FindFi.Ef.Bll.DTOs;

public class ProductQuery
{
    // Paging
    public int Page { get; set; } = 1; // starts from 1
    public int PageSize { get; set; } = 10; // default

    // Filtering
    public string? Search { get; set; } // matches Code or Name (contains)
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }

    // Sorting
    public string? SortBy { get; set; } // code|name|price|createdAt
    public string? SortDir { get; set; } // asc|desc

    public const int MaxPageSize = 100;

    public void Normalize()
    {
        if (Page < 1) Page = 1;
        if (PageSize <= 0) PageSize = 10;
        if (PageSize > MaxPageSize) PageSize = MaxPageSize;
        SortBy = string.IsNullOrWhiteSpace(SortBy) ? "createdAt" : SortBy.Trim();
        SortDir = string.IsNullOrWhiteSpace(SortDir) ? "desc" : SortDir.Trim().ToLowerInvariant();
    }
}
