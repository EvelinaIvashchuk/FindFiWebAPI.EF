using FindFi.Ef.Bll.DTOs;

namespace FindFi.Ef.Bll.Abstractions;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<ProductDto>> GetPagedAsync(ProductQuery query, CancellationToken cancellationToken = default);
    Task<ProductDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<long> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(long id, UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}