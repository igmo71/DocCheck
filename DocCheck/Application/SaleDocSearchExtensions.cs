using DocCheck.Domain;

namespace DocCheck.Application
{
    public static class SaleDocSearchExtensions
    {
        public static IQueryable<SaleDoc> HandleSearch(this IQueryable<SaleDoc> query, SaleDocSearchParams searchParams)
        {
            if (searchParams.Customer is not null)
                query = query.Where(e => e.CustomerName != null && e.CustomerName.ToLower().Contains(searchParams.Customer.ToLower()));

            if (searchParams.SaleDocNumber is not null)
                query = query.Where(e => e.Number != null && e.Number.ToLower().Contains(searchParams.SaleDocNumber.ToLower()));

            

            return query;
        }

    }
}
