using DocCheck.Common;
using DocCheck.Data;
using DocCheck.Domain;
using DocCheck.Infrastructure.OData;
using DocCheck.Infrastructure.OData.Models;
using DocCheck.Infrastructure.Whs.Models;
using Microsoft.EntityFrameworkCore;

namespace DocCheck.Application
{
    public interface ISaleDocService
    {
        Task CreateAsync(string mngrOrderString);
        Task UpdateAsync(SaleDoc saleDoc);
        Task<List<SaleDoc>> GetListAsync();
        Task<List<SaleDoc>?> GetListByBarcodeAsync(string barcode);
        Task<SaleDoc?> GetAsync(Guid id);
        Task<ServiceResult<SaleDoc>> LoadAsync(Guid id, bool isOpenByBarcode);
        Task DeleteUserAsync(string userId);
    }

    public class SaleDocService(
       ApplicationDbContext dbContext,
       IODataService oDataService,
       AuthService authService,
       ILogger<SaleDocService> logger) : ISaleDocService
    {
        public async Task CreateAsync(string mngrOrderString)
        {
            var mngrDocuments = MngrOrder.From(mngrOrderString);

            if (mngrDocuments is null)
            {
                logger.LogWarning("{Source} baseDocuments is null", nameof(CreateAsync));
                return;
            }

            var saleDocs = mngrDocuments
                .Where(e => e.Распоряжение_Name != null &&
                            e.Распоряжение_Name.Contains(Document_РеализацияТоваровУслуг.DocumentName))
                .Select(e => SaleDoc.From(e));

            foreach (var doc in saleDocs)
            {
                if (dbContext.SaleDocs.Any(e => e.Id == doc.Id))
                {
                    logger.LogDebug("{Source} Exists {@SaleDoc}", nameof(CreateAsync), doc);
                }
                else
                {
                    doc.PositionId = Position.ForDispatch.Id;
                    await dbContext.SaleDocs.AddAsync(doc);
                    logger.LogDebug("{Source} Add {@SaleDoc}", nameof(CreateAsync), doc);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(SaleDoc item)
        {
            item.UserId = await authService.GetCurrentUserIdAsync();

            UpdatePositionWhenSubmit(item);

            dbContext.SaleDocLogs.Add(new SaleDocLog(item));

            dbContext.Update(item);

            await dbContext.SaveChangesAsync();
        }

        private static void UpdatePositionWhenSubmit(SaleDoc saleDoc)
        {
            if (saleDoc.IsCorrect)
            {
                if (saleDoc.Position == Position.Operators)
                    saleDoc.PositionId = Position.Accounting.Id;

                if (saleDoc.Position == Position.Managers)
                {
                    saleDoc.PositionId = Position.ForDispatch.Id;
                    saleDoc.Redispatch++;
                }

                if (saleDoc.Position == Position.Accounting)
                    saleDoc.PositionId = Position.Closed.Id;

                if (saleDoc.Position == Position.Closed)
                    saleDoc.PositionId = Position.ForDispatch.Id;
            }
            else
            {
                saleDoc.PositionId = Position.Managers.Id;
            }
        }

        public async Task<SaleDoc?> GetAsync(Guid id)
        {
            var item = await dbContext.SaleDocs
                .Include(e => e.PaperworkErrors)
                .Include(e => e.QuantityErrors)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (item is null)
                return null;

            return item;
        }

        public async Task<ServiceResult<SaleDoc>> LoadAsync(Guid id, bool isOpenByBarcode)
        {
            var item = await GetAsync(id);

            if (isOpenByBarcode)
                await UpdatePositionWhenOpenByBarcodeAsync(item);
            else
            {
                if (!await authService.IsCurrentUserInRole(item.Position.Role))
                    return Error.MismatchedRole;
            }

            item.User = await authService.FindByIdAsync(item.UserId);

            return item;
        }


        private async Task UpdatePositionWhenOpenByBarcodeAsync(SaleDoc item)
        {
            if (await authService.IsCurrentUserInRole(Position.Operators.Role))
                item.PositionId = Position.Operators.Id;
            if (await authService.IsCurrentUserInRole(Position.Managers.Role))
                item.PositionId = Position.Managers.Id;
            if (await authService.IsCurrentUserInRole(Position.Accounting.Role))
                item.PositionId = Position.Accounting.Id;
        }

        public async Task<List<SaleDoc>> GetListAsync()
        {
            var result = await dbContext.SaleDocs
                .AsNoTracking()
                .Where(e => e.PositionId != Position.Closed.Id)
                .OrderByDescending(e => e.Date)
                .Take(1000)
                .ToListAsync();

            return result;
        }        

        public async Task<List<SaleDoc>?> GetListByBarcodeAsync(string barcode)
        {
            var invoiceRefKey = GuidConvert.FromNumStr(barcode);

            var baseDocuments = await oDataService.GetDocument_СчетФактураВыданный_ДокументыОснования(invoiceRefKey);

            if (baseDocuments is null)
                return null;

            var baseDocumentsStr = string.Join(" ", baseDocuments.Select(d => d.ДокументОснование));

            var saleDocs = await dbContext.SaleDocs
                 .AsNoTracking()
                 .Where(saleDoc => baseDocumentsStr.Contains(saleDoc.Id.ToString()))
                 .ToListAsync();

            if (saleDocs is null || saleDocs.Count == 0)
                saleDocs = await TryLoadAndCreateAsync(baseDocuments, saleDocs);

            return saleDocs;
        }

        private async Task<List<SaleDoc>?> TryLoadAndCreateAsync(Document_СчетФактураВыданный_ДокументыОснования[] baseDocuments, List<SaleDoc>? saleDocs)
        {
            saleDocs ??= [];

            foreach (var baseDocument in baseDocuments)
            {
                var document = await oDataService.GetDocument_РеализацияТоваровУслуг(baseDocument.ДокументОснование);
                if (document is null)
                    continue;

                saleDocs.Add(SaleDoc.From(document));
            }

            if (saleDocs is not null && saleDocs.Count > 0)
            {
                await dbContext.SaleDocs.AddRangeAsync(saleDocs);

                await dbContext.SaveChangesAsync();
            }

            return saleDocs;
        }

        public async Task DeleteUserAsync(string userId)
        {
            await dbContext.SaleDocs
                .Where(e => e.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
