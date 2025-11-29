using AutoMapper;
using FindFi.Ef.Bll.Abstractions;
using FindFi.Ef.Bll.DTOs;
using FindFi.Ef.Data.Abstractions;
using FindFi.Ef.Data.Specifications;
using FindFi.Ef.Domain.Entities;

namespace FindFi.Ef.Bll.Services;

public class ListingService : IListingService
{
    private readonly IAsyncRepository<Listing> _repo;
    private readonly IMapper _mapper;

    public ListingService(IAsyncRepository<Listing> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PagedResult<ListingDto>> GetListingsAsync(ListingQuery query, CancellationToken cancellationToken = default)
    {
        query.Normalize();

        var skip = (query.Page - 1) * query.PageSize;
        var spec = new ListingFilterSpecification(
            query.City,
            query.PropertyType,
            query.ListingType,
            query.MinPrice,
            query.MaxPrice,
            query.Tags,
            query.DateFrom,
            query.DateTo,
            query.SortBy!,
            query.SortDir!,
            skip,
            query.PageSize);

        var items = await _repo.ListAsync(spec, cancellationToken);
        var total = await _repo.CountAsync(new ListingFilterSpecification(
            query.City, query.PropertyType, query.ListingType,
            query.MinPrice, query.MaxPrice, query.Tags,
            query.DateFrom, query.DateTo,
            query.SortBy!, query.SortDir!,
            0, 0), cancellationToken);

        var dtoItems = items.Select(_mapper.Map<ListingDto>).ToList();

        return new PagedResult<ListingDto>
        {
            Items = dtoItems,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<ListingDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var spec = new ListingByIdSpecification(id);
        var list = await _repo.ListAsync(spec, cancellationToken);
        var entity = list.FirstOrDefault();
        return entity == null ? null : _mapper.Map<ListingDto>(entity);
    }

    public Task<int> GetListingCount(CancellationToken cancellationToken = default)
    {
        return _repo.CountAsync(cancellationToken);
    }
}
