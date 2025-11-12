using System.Threading;
using System.Threading.Tasks;
using FindFi.Ef.Bll.Abstractions;
using FindFi.Ef.Bll.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FindFi.Ef.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _service;

    public TagsController(ITagService service)
    {
        _service = service;
    }

    // GET /api/tags
    [HttpGet]
    public async Task<ActionResult<TagDto[]>> GetAll(CancellationToken ct)
    {
        var dtos = await _service.GetAllAsync(ct);
        return Ok(dtos);
    }
}
