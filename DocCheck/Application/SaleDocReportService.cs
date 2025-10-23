using DocCheck.Data;
using DocCheck.Domain;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.EntityFrameworkCore;

namespace DocCheck.Application
{
    public interface ISaleDocReportService
    {
        SaleDoc[] BuildReport(ReportParams reportParams);
        GridItemsProviderResult<SaleDoc> BuildReport(GridItemsProviderRequest<SaleDoc> gridRequest, ReportParams reportParams);
        Dictionary<string, string> GetManagers();
    }

    public class ReportParams
    {
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public List<int>? PositionId { get; set; }
        public string? ManagerId { get; set; }
        public string? CustomerTerm { get; set; }
        public string? SaleDocTerm { get; set; }
    }

    public class SaleDocReportService(IDbContextFactory<ApplicationDbContext> dbFactory) : ISaleDocReportService
    {
        public GridItemsProviderResult<SaleDoc> BuildReport(GridItemsProviderRequest<SaleDoc> gridRequest, ReportParams reportParams)
        {
            using var dbContext = dbFactory.CreateDbContext();

            var query = dbContext.SaleDocs
                .AsNoTracking()
                .HandleReportParams(reportParams);

            var totalItemCount = query
                .Count();

            var items = query
                .Include(e => e.PaperworkErrors)
                .HandleGridRequest(gridRequest)
                .ToArray();

            var gridResult = new GridItemsProviderResult<SaleDoc>()
            {
                TotalItemCount = totalItemCount,
                Items = items
            };

            return gridResult;
        }

        public SaleDoc[] BuildReport(ReportParams reportParams)
        {
            using var dbContext = dbFactory.CreateDbContext();

            var result = dbContext.SaleDocs
                .AsNoTracking()
                .HandleReportParams(reportParams)
                .Include(e => e.PaperworkErrors)
                .ToArray();

            return result;
        }

        public Dictionary<string, string> GetManagers()
        {
            using var dbContext = dbFactory.CreateDbContext();

            var result = dbContext.SaleDocs
                .Where(e => e.ManagerId != null && e.ManagerName != null)
                .GroupBy(e => new { e.ManagerId, e.ManagerName })
                .Select(g => new { g.Key.ManagerId, g.Key.ManagerName })
                .OrderBy(e => e.ManagerName)
                .ToDictionary(e => e.ManagerId!.ToString(), e => e.ManagerName!);

            return result;
        }
    }
}
