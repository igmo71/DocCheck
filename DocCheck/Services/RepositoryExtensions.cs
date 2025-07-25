using DocCheck.Models;

namespace DocCheck.Services
{
    public static class RepositoryExtensions
    {
        public static IQueryable<DocumentCheck> HandleFilterQuery(this IQueryable<DocumentCheck> query, SearchParams searchParams)
        {
            if (searchParams.RefKey is not null)
                query = query.Where(e => e.InvoiceRefKey == searchParams.RefKey);

            if (searchParams.Number is not null)
                query = query.Where(e => e.InvoiceNumber == searchParams.Number);

            if (searchParams.DateFrom is not null)
                query = query.Where(e => e.InvoiceDate.Date >= searchParams.DateFrom);

            if (searchParams.DateTo is not null)
                query = query.Where(e => e.InvoiceDate.Date < ((DateTime)searchParams.DateTo).AddDays(1));

            return query;
        }

        public static IQueryable<DocumentCheck> HandleQuery(this IQueryable<DocumentCheck> query, SearchParams searchParams)
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
