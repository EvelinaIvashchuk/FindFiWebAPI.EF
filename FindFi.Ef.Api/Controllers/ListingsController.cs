using System.Threading;
using System.Threading.Tasks;
using FindFi.Ef.Bll.Abstractions;
using FindFi.Ef.Bll.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FindFi.Ef.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController(IListingService service) : ControllerBase
{
    // GET /api/listings?city=Kyiv&propertyType=Apartment&listingType=LongTerm&minPrice=10000&tags=wifi&tags=parking&page=1&pageSize=12&sortBy=price&sortDir=asc
    [HttpGet]
    public async Task<ActionResult<PagedResult<ListingDto>>> GetListings([FromQuery] ListingQuery query, CancellationToken ct)
    {
        var result = await service.GetListingsAsync(query, ct);
        return Ok(result);
    }

    // GET /api/listings/{id}
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ListingDto>> GetById(long id, CancellationToken ct)
    {
        var dto = await service.GetByIdAsync(id, ct);
        if (dto == null) return NotFound();
        return Ok(dto);
    }
}
