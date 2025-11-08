using AutoMapper;
using FindFi.Ef.Bll.Abstractions;
using FindFi.Ef.Bll.DTOs;
using FindFi.Ef.Data.Abstractions;
using FindFi.Ef.Domain.Entities;
using FindFi.Ef.Domain.Exceptions;

namespace FindFi.Ef.Bll.Services;

public class ProductService(IEfUnitOfWork uow, IMapper mapper) : IProductService
{
    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await uow.Products.GetAllAsync(cancellationToken);
        return mapper.Map<IEnumerable<ProductDto>>(items);
    }

    public async Task<PagedResult<ProductDto>> GetPagedAsync(ProductQuery query, CancellationToken cancellationToken = default)
    {
        query.Normalize();
        var skip = (query.Page - 1) * query.PageSize;
        var sortDesc = string.Equals(query.SortDir, "desc", StringComparison.OrdinalIgnoreCase);
        var spec = new FindFi.Ef.Data.Specifications.ProductFilterSpec(
            query.Search, query.MinPrice, query.MaxPrice, query.IsActive, query.CreatedFrom, query.CreatedTo,
            query.SortBy ?? "createdAt", sortDesc, skip, query.PageSize);

        var list = await uow.Products.ListAsync(spec, cancellationToken);
        var total = await uow.Products.CountAsync(new FindFi.Ef.Data.Specifications.ProductFilterSpec(
            query.Search, query.MinPrice, query.MaxPrice, query.IsActive, query.CreatedFrom, query.CreatedTo,
            query.SortBy ?? "createdAt", sortDesc, 0, 0), cancellationToken);

        var dtos = mapper.Map<List<ProductDto>>(list);
        return new PagedResult<ProductDto>
        {
            Items = dtos,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<ProductDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await uow.Products.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            throw new NotFoundException($"Product {id} not found");
        return mapper.Map<ProductDto>(entity);
    }

    public async Task<long> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        Validate(dto);
        // Uniqueness: Code must be unique
        var existingCode = await uow.Products.GetByCodeAsync(dto.Code, cancellationToken);
        if (existingCode != null)
        {
            throw new ValidationException("Product validation failed", new Dictionary<string, string[]>
            {
                ["Code"] = ["Code must be unique"]
            });
        }

        var entity = mapper.Map<Product>(dto);
        await uow.Products.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(long id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        Validate(dto);
        var entity = await uow.Products.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            throw new NotFoundException($"Product {id} not found");

        // If code is changing, ensure unique
        if (!string.Equals(entity.Code, dto.Code, StringComparison.OrdinalIgnoreCase))
        {
            var byCode = await uow.Products.GetByCodeAsync(dto.Code, cancellationToken);
            if (byCode != null && byCode.Id != id)
            {
                throw new ValidationException("Product validation failed", new Dictionary<string, string[]>
                {
                    ["Code"] = ["Code must be unique"]
                });
            }
        }

        entity.Code = dto.Code;
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Price = dto.Price;
        entity.IsActive = dto.IsActive;

        await uow.Products.UpdateAsync(entity, cancellationToken);
        var affected = await uow.SaveChangesAsync(cancellationToken);
        if (affected <= 0)
        {
            throw new BusinessConflictException($"Failed to update product {id}");
        }
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await uow.Products.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            throw new NotFoundException($"Product {id} not found");

        await uow.Products.DeleteAsync(entity, cancellationToken);
        var affected = await uow.SaveChangesAsync(cancellationToken);
        if (affected <= 0)
        {
            throw new BusinessConflictException($"Failed to delete product {id}");
        }
    }

    private static void Validate(CreateProductDto dto)
    {
        var errors = new Dictionary<string, string[]>();
        if (string.IsNullOrWhiteSpace(dto.Code))
            errors["Code"] = ["Code is required"]; 
        else if (dto.Code.Length > 64)
            errors["Code"] = ["Code length must be <= 64 characters"]; 

        if (string.IsNullOrWhiteSpace(dto.Name))
            errors["Name"] = ["Name is required"]; 
        else if (dto.Name.Length > 200)
            errors["Name"] = ["Name length must be <= 200 characters"]; 

        if (dto.Description != null && dto.Description.Length > 1000)
            errors["Description"] = ["Description length must be <= 1000 characters"]; 

        if (dto.Price < 0)
            errors["Price"] = ["Price must be non-negative"]; 

        if (errors.Count > 0)
            throw new ValidationException("Product validation failed", errors);
    }

    private static void Validate(UpdateProductDto dto)
    {
        var errors = new Dictionary<string, string[]>();
        if (string.IsNullOrWhiteSpace(dto.Code))
            errors["Code"] = ["Code is required"]; 
        else if (dto.Code.Length > 64)
            errors["Code"] = ["Code length must be <= 64 characters"]; 

        if (string.IsNullOrWhiteSpace(dto.Name))
            errors["Name"] = ["Name is required"]; 
        else if (dto.Name.Length > 200)
            errors["Name"] = ["Name length must be <= 200 characters"]; 

        if (dto.Description != null && dto.Description.Length > 1000)
            errors["Description"] = ["Description length must be <= 1000 characters"]; 

        if (dto.Price < 0)
            errors["Price"] = ["Price must be non-negative"]; 

        if (errors.Count > 0)
            throw new ValidationException("Product validation failed", errors);
    }
}
