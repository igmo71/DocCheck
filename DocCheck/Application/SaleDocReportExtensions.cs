using DocCheck.Domain;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DocCheck.Application
{
    public static class SaleDocReportExtensions
    {
        public static IQueryable<SaleDoc> HandleReportParams(this IQueryable<SaleDoc> query, ReportParams reportParams)
        {
            if (reportParams.DateBegin is not null)
                query = query.Where(e => e.Date >= reportParams.DateBegin);

            if (reportParams.DateEnd is not null)
                query = query.Where(e => e.Date < reportParams.DateEnd);

            if (reportParams.PositionId is not null && reportParams.PositionId.Count > 0)
            {
                var ids = reportParams.PositionId.ToArray();
                //query = query.Where(e => ids.Contains(e.PositionId));
                query = query.Where("@0.Contains(PositionId)", ids);
            }

            if (!string.IsNullOrEmpty(reportParams.ManagerId))
                query = query.Where(e => e.ManagerId == reportParams.ManagerId);

            if (!string.IsNullOrEmpty(reportParams.CustomerTerm))
                query = query.Where(e => e.CustomerName != null && e.CustomerName.ToLower().Contains(reportParams.CustomerTerm.ToLower()));

            if (!string.IsNullOrEmpty(reportParams.SaleDocTerm))
                query = query.Where(e => e.Number != null && e.Number.ToLower().Contains(reportParams.SaleDocTerm.ToLower()));

            return query;
        }

        public static IQueryable<SaleDoc> HandleGridRequest(this IQueryable<SaleDoc> query, GridItemsProviderRequest<SaleDoc> gridRequest)
        {
            // sorting
            var sortedProperties = gridRequest.GetSortByProperties().ToList();

            if (sortedProperties.Count == 0)
                query = query.OrderByDescending(e => e.Date);
            else if (sortedProperties.Count > 0)
            {
                var firstSortedProperty = sortedProperties.First();

                query = firstSortedProperty.Direction == SortDirection.Ascending
                    ? query.OrderBy(e => EF.Property<object>(e, firstSortedProperty.PropertyName))
                    : query.OrderByDescending(e => EF.Property<object>(e, firstSortedProperty.PropertyName));

                foreach (var sortedProperty in sortedProperties.Skip(1))
                {
                    query = sortedProperty.Direction == SortDirection.Ascending
                        ? ((IOrderedQueryable<SaleDoc>)query).ThenBy(e => EF.Property<object>(e, sortedProperty.PropertyName))
                        : ((IOrderedQueryable<SaleDoc>)query).ThenByDescending(e => EF.Property<object>(e, sortedProperty.PropertyName));
                }
            }

            // pagination
            if (gridRequest.StartIndex > 0)
                query = query.Skip(gridRequest.StartIndex);

            if (gridRequest.Count is > 0)
                query = query.Take(gridRequest.Count.Value);

            return query;
        }
    }
}
