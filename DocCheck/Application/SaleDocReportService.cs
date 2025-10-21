using DocCheck.Data;
using DocCheck.Domain;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.EntityFrameworkCore;

namespace DocCheck.Application
{
    public interface ISaleDocReportService
    {
        Task<GridItemsProviderResult<SaleDoc>> BuildReport(GridItemsProviderRequest<SaleDoc> gridRequest, ReportParams reportParams);
        Task<Dictionary<string, string>> GetManagers();
    }

    public class ReportParams
    {
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? PositionId { get; set; }
        public string? ManagerId { get; set; }
        public string? CustomerTerm { get; set; }
        public string? SaleDocTerm { get; set; }
    }

    public class SaleDocReportService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<SaleDocReportService> logger) : ISaleDocReportService
    {
        public async Task<GridItemsProviderResult<SaleDoc>> BuildReport(GridItemsProviderRequest<SaleDoc> gridRequest, ReportParams reportParams)
        {
            await using var dbContext = await dbFactory.CreateDbContextAsync();

            var totalItemCount = await dbContext.SaleDocs.CountAsync();

            var items = await dbContext.SaleDocs
                .AsNoTracking()
                .Include(e => e.PaperworkErrors)
                .HandleReportParams(reportParams)
                .HandleGridRequest(gridRequest)
                .AsNoTracking()
                .ToArrayAsync();

            var gridResult = new GridItemsProviderResult<SaleDoc>()
            {
                TotalItemCount = totalItemCount,
                Items = items
            };

            return gridResult;
        }

        public async Task<Dictionary<string, string>> GetManagers()
        {
            await using var dbContext = await dbFactory.CreateDbContextAsync();

            var result = await dbContext.SaleDocs
                .Where(e => e.ManagerId != null && e.ManagerName != null)
                .GroupBy(e => new { e.ManagerId, e.ManagerName })
                .Select(g => new
                {
                    ManagerId = g.Key.ManagerId,
                    ManagerName = g.Key.ManagerName
                })
                .OrderBy(e => e.ManagerName)
                .ToDictionaryAsync(e => e.ManagerId!.ToString(), e => e.ManagerName!);

            return result;
        }
    }
}
