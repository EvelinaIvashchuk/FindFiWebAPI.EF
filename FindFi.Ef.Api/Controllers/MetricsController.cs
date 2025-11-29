using FindFi.Ef.Bll.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FindFi.Ef.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetricsController(IListingService service) : ControllerBase
{
    [HttpGet("listing-count")]
    public async Task<ActionResult<int>> GetListingCount(CancellationToken ct)
    {
       return await service.GetListingCount(ct);
    }
}