using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FindFi.Ef.Bll.Abstractions;
using FindFi.Ef.Bll.DTOs;
using FindFi.Ef.Data.Abstractions;
using FindFi.Ef.Domain.Entities;

namespace FindFi.Ef.Bll.Services;

public class TagService : ITagService
{
    private readonly IAsyncRepository<Tag> _repo;
    private readonly IMapper _mapper;

    public TagService(IAsyncRepository<Tag> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<TagDto[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var tags = await _repo.GetAllAsync(cancellationToken);
        return tags.Select(_mapper.Map<TagDto>).ToArray();
    }
}
