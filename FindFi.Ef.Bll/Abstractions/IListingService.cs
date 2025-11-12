using System.Threading;
using System.Threading.Tasks;
using FindFi.Ef.Bll.DTOs;

namespace FindFi.Ef.Bll.Abstractions;

public interface IListingService
{
    Task<PagedResult<ListingDto>> GetListingsAsync(ListingQuery query, CancellationToken cancellationToken = default);
    Task<ListingDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}
