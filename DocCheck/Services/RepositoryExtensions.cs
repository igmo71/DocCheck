using DocCheck.Common;
using System.Linq;

namespace DocCheck.Services
{
    public static class RepositoryExtensions
    {
        public static IQueryable<TEntity> HandleFilterQuery<TEntity>(this IQueryable<TEntity> query, SearchParams searchParams)
            where TEntity : class, IHasDate, IHasNumber, IHasRefKey
        {
            if (searchParams.RefKey is not null)
                query = query.Where(e => e.RefKey == searchParams.RefKey);

            if (searchParams.Number is not null)
                query = query.Where(e => e.Number == searchParams.Number);

            if (searchParams.DateFrom is not null)
                query = query.Where(e => e.Date.Date >= searchParams.DateFrom);

            if (searchParams.DateTo is not null)
                query = query.Where(e => e.Date.Date < ((DateTime)searchParams.DateTo).AddDays(1));

            return query;
        }

        public static IQueryable<TEntity> HandleQuery<TEntity>(this IQueryable<TEntity> query, SearchParams searchParams)
            where TEntity : class, IHasDate, IHasNumber, IHasRefKey
        {
            query = query.HandleFilterQuery(searchParams);

            if (searchParams.Skip > 0)
                query = query.Skip(searchParams.Skip);

            if (searchParams.Take > 0)
                query = query.Take((int)searchParams.Take);

            return query;
        }
    }
}
