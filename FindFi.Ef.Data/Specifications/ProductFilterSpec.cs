using System;
using System.Linq.Expressions;
using FindFi.Ef.Data.Abstractions;
using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FindFi.Ef.Data.Specifications;

public class ProductFilterSpec : BaseSpecification<Product>
{
    public ProductFilterSpec(
        string? search,
        decimal? minPrice,
        decimal? maxPrice,
        bool? isActive,
        DateTime? createdFrom,
        DateTime? createdTo,
        string sortBy,
        bool sortDesc,
        int skip,
        int take)
    {
        // Criteria
        Expression<Func<Product, bool>>? criteria = null;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search.Trim()}%";
            criteria = p => EF.Functions.Like(p.Name, term) || EF.Functions.Like(p.Code, term);
        }

        if (minPrice.HasValue)
        {
            var prev = criteria;
            criteria = prev is null ? p => p.Price >= minPrice.Value : prev.AndAlso(p => p.Price >= minPrice.Value);
        }
        if (maxPrice.HasValue)
        {
            var prev = criteria;
            criteria = prev is null ? p => p.Price <= maxPrice.Value : prev.AndAlso(p => p.Price <= maxPrice.Value);
        }
        if (isActive.HasValue)
        {
            var prev = criteria;
            criteria = prev is null ? p => p.IsActive == isActive.Value : prev.AndAlso(p => p.IsActive == isActive.Value);
        }
        if (createdFrom.HasValue)
        {
            var prev = criteria;
            criteria = prev is null ? p => p.CreatedAt >= createdFrom.Value : prev.AndAlso(p => p.CreatedAt >= createdFrom.Value);
        }
        if (createdTo.HasValue)
        {
            var prev = criteria;
            criteria = prev is null ? p => p.CreatedAt <= createdTo.Value : prev.AndAlso(p => p.CreatedAt <= createdTo.Value);
        }

        Criteria = criteria;

        // Sorting
        sortBy = (sortBy ?? "createdAt").ToLowerInvariant();
        if (sortDesc)
        {
            ApplyOrderByDescending(sortBy switch
            {
                "code" => p => p.Code,
                "name" => p => p.Name,
                "price" => p => p.Price,
                _ => p => p.CreatedAt
            });
        }
        else
        {
            ApplyOrderBy(sortBy switch
            {
                "code" => p => p.Code,
                "name" => p => p.Name,
                "price" => p => p.Price,
                _ => p => p.CreatedAt
            });
        }

        // Paging
        if (skip > 0 || take > 0)
        {
            if (skip < 0) skip = 0;
            if (take <= 0) take = 10;
            ApplyPaging(skip, take);
        }
    }
}

internal static class PredicateBuilder
{
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body)!;

        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body)!;

        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
    }

    private class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression? Visit(Expression? node)
        {
            if (node == _oldValue)
                return _newValue;
            return base.Visit(node);
        }
    }
}
