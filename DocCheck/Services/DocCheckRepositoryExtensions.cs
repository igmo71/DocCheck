using DocCheck.Models;
using System.Linq.Expressions;

namespace DocCheck.Services
{
    public static class DocCheckRepositoryExtensions
    {
        public static IQueryable<DocumentCheck> HandleQuery(this IQueryable<DocumentCheck> query, SearchParams searchParams)
        {
            query = query.HandleFilterQuery(searchParams);

            query = query.ApplyOrder(searchParams);

            if (searchParams.Skip > 0)
                query = query.Skip(searchParams.Skip);

            if (searchParams.Take > 0)
                query = query.Take((int)searchParams.Take);

            return query;
        }

        public static IQueryable<DocumentCheck> HandleFilterQuery(this IQueryable<DocumentCheck> query, SearchParams searchParams)
        {

            if (searchParams.RefKey is not null)
                query = query.Where(e => e.InvoiceRefKey == searchParams.RefKey);

            if (!string.IsNullOrWhiteSpace(searchParams.Number))
                query = query.Where(e => e.InvoiceNumber != null && e.InvoiceNumber.ToLower().Contains(searchParams.Number.ToLower()));

            if (searchParams.DateFrom is not null)
                query = query.Where(e => e.InvoiceDate.Date >= searchParams.DateFrom);

            if (searchParams.DateTo is not null)
                query = query.Where(e => e.InvoiceDate.Date < ((DateTime)searchParams.DateTo).AddDays(1));

            if (!string.IsNullOrWhiteSpace(searchParams.UserId))
                query = query.Where(e => e.UserId == searchParams.UserId);

            if (!searchParams.IsShowClosed)
                query = query.Where(e => e.Status != Status.Closed);

            return query;
        }

        public static IQueryable<DocumentCheck> ApplyOrder(this IQueryable<DocumentCheck> query, SearchParams searchParams)
        {
            if (searchParams.OrderBy is null)
                return query;

            var parameter = Expression.Parameter(typeof(DocumentCheck), "x");
            var property = Expression.Property(parameter, searchParams.OrderBy);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = searchParams.IsOrderAsc ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                [typeof(DocumentCheck), property.Type],
                query.Expression,
                Expression.Quote(lambda));

            return query.Provider.CreateQuery<DocumentCheck>(resultExpression);
        }
    }
}
