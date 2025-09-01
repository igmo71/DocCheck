using DocCheck.Data;
using DocCheck.Domain;
using Microsoft.EntityFrameworkCore;

namespace DocCheck.Application
{
    public interface ISaleDocLogService
    {
        Task<List<SaleDocLog>?> GetList(Guid saleDocId);
        Task<SaleDocLog?> Get(Guid id);
    }

    public class SaleDocLogService(ApplicationDbContext dbContext) : ISaleDocLogService
    {
        public async Task<SaleDocLog?> Get(Guid id)
        {
            var result = await dbContext.SaleDocLogs
                .AsNoTracking()
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            return result;
        }

        public async Task<List<SaleDocLog>?> GetList(Guid saleDocId)
        {
            var result = await dbContext.SaleDocLogs
                .AsNoTracking()
                .Include(e => e.User)
                .Where(e => e.SaleDocId == saleDocId)
                .ToListAsync();

            return result;
        }
    }
}
