using DocCheck.Common;
using System.Linq.Expressions;

namespace DocCheck.Services
{
    public static class RepositoryExtensions
    {
        public static IQueryable<TEntity> HandleFilterQuery<TEntity>(this IQueryable<TEntity> query, SearchParams searchParams)
            where TEntity : class, IHasInvoice
        {
            if (searchParams.RefKey is not null)
                query = query.Where(e => e.InvoiceRefKey == searchParams.RefKey);

            if (searchParams.Number is not null)
                query = query.Where(e => e.InvoiceNumber != null && e.InvoiceNumber.ToLower().Contains(searchParams.Number.ToLower()));

            if (searchParams.DateFrom is not null)
                query = query.Where(e => e.InvoiceDate.Date >= searchParams.DateFrom);

            if (searchParams.DateTo is not null)
                query = query.Where(e => e.InvoiceDate.Date < ((DateTime)searchParams.DateTo).AddDays(1));

            return query;
        }

        public static IQueryable<TEntity> HandleQuery<TEntity>(this IQueryable<TEntity> query, SearchParams searchParams)
            where TEntity : class, IHasInvoice
        {
            query = query.HandleFilterQuery(searchParams);

            query = query.ApplyOrder(searchParams);

            if (searchParams.Skip > 0)
                query = query.Skip(searchParams.Skip);

            if (searchParams.Take > 0)
                query = query.Take((int)searchParams.Take);

            return query;
        }

        public static IQueryable<TEntity> ApplyOrder<TEntity>(this IQueryable<TEntity> query, SearchParams searchParams)
        {
            if (searchParams.OrderBy is null)
                return query;

            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, searchParams.OrderBy);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = searchParams.IsOrderAsc ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                [typeof(TEntity), property.Type],
                query.Expression,
                Expression.Quote(lambda));

            return query.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}