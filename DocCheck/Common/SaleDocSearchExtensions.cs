using DocCheck.Domain;

namespace DocCheck.Common
{
    public static class SaleDocSearchExtensions
    {
        public static IQueryable<SaleDoc> HandleSearch(this IQueryable<SaleDoc> query, SearchParams searchParams)
        {
            if (searchParams.Customer is not null)
                query = query.Where(e => e.CustomerName != null && e.CustomerName.ToLower().Contains(searchParams.Customer.ToLower()));

            if (searchParams.SaleDocNumber is not null)
                query = query.Where(e => e.Number != null && e.Number.ToLower().Contains(searchParams.SaleDocNumber.ToLower()));

            

            return query;
        }

    }
}
