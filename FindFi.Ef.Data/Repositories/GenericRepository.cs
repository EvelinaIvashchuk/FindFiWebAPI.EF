using FindFi.Ef.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FindFi.Ef.Data.Repositories;

public class GenericRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _db;
    protected readonly DbSet<TEntity> _set;

    public GenericRepository(AppDbContext db)
    {
        _db = db;
        _set = _db.Set<TEntity>();
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _set.FindAsync([id], cancellationToken);
    }

    public virtual Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _set.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _set.Attach(entity);
        _db.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _set.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _db.SaveChangesAsync(cancellationToken);

    // Specification support
    public virtual async Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification, forCount: true);
        return query.CountAsync(cancellationToken);
    }

    protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec, bool forCount = false)
    {
        IQueryable<TEntity> query = _set;
        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);
        foreach (var include in spec.Includes)
            query = query.Include(include);
        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);
        else if (spec.OrderByDescending != null)
            query = query.OrderByDescending(spec.OrderByDescending);
        if (!forCount && spec.Skip.HasValue)
            query = query.Skip(spec.Skip.Value);
        if (!forCount && spec.Take.HasValue)
            query = query.Take(spec.Take.Value);
        return query;
    }
}
