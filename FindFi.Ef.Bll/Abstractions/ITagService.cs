using System.Threading;
using System.Threading.Tasks;
using FindFi.Ef.Bll.DTOs;

namespace FindFi.Ef.Bll.Abstractions;

public interface ITagService
{
    Task<TagDto[]> GetAllAsync(CancellationToken cancellationToken = default);
}
