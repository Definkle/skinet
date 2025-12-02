using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        if (spec.IsDistinct)
        {
            query = query.Distinct();
        }

        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }

    public static IQueryable<TResult> GetQuery<TResult>(IQueryable<T> query, ISpecification<T, TResult> spec)
    {
        var baseQuery = query;

        if (spec.Criteria != null)
        {
            baseQuery = baseQuery.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            baseQuery = baseQuery.OrderBy(spec.OrderBy);
        }

        if (spec.OrderByDescending != null)
        {
            baseQuery = baseQuery.OrderByDescending(spec.OrderByDescending);
        }

        var selectQuery = spec.Select != null
            ? baseQuery.Select(spec.Select)
            : baseQuery.Cast<TResult>();

        if (spec.IsDistinct)
        {
            selectQuery = selectQuery.Distinct();
        }

        return selectQuery;
    }
}
