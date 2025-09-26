using DocCheck.Common;
using DocCheck.Data;
using DocCheck.Domain;
using DocCheck.Infrastructure.OData;
using DocCheck.Infrastructure.OData.Models;
using DocCheck.Infrastructure.Whs.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DocCheck.Application
{
    public interface ISaleDocService
    {
        Task CreateIfNotExistsAsync(SaleDoc saleDoc);
        Task<SaleDoc?> CreateByInvoiceAsync(string invoiceRefKey);
        Task CreateByBaseDocAsync(string mngrOrderString);
        Task UpdateAsync(SaleDoc item);
        Task UpdateBySubmitAsync(SaleDoc saleDoc);
        Task<List<SaleDoc>> GetListUnclosedAsync(SearchParams searchParams);
        Task<SaleDoc?> GetAsync(Guid id);
        Task<ServiceResult<SaleDoc>> GetByAccessRightsAsync(Guid id);
        Task<ServiceResult<SaleDoc>> GetByBarcodeAsync(string barcode);
        Task<SaleDoc?> GetByTaskAsync(Guid taskId);
        Task DeleteUserAsync(string userId);
        Task DeleteAsync(Guid id);

        Task ActualizeAsync();

        string?[]? GetCustomers();
        Task HandleTaskAsync(string message);
    }

    public class SaleDocService(
       ApplicationDbContext dbContext,
       IODataService oDataService,
       AuthService authService,
       ILogger<SaleDocService> logger) : ISaleDocService
    {
        public async Task CreateIfNotExistsAsync(SaleDoc saleDoc)
        {
            if (dbContext.SaleDocs.Any(e => e.Id == saleDoc.Id))
            {
                logger.LogDebug("{Source} Exists {@SaleDoc}", nameof(CreateIfNotExistsAsync), saleDoc);
                return;
            }

            saleDoc.PositionId = Position.ForDispatch.Id;

            await dbContext.SaleDocs.AddAsync(saleDoc);

            logger.LogDebug("{Source} Add {@SaleDoc}", nameof(CreateIfNotExistsAsync), saleDoc);

            await dbContext.SaveChangesAsync();
        }

        public async Task<SaleDoc?> CreateByInvoiceAsync(string invoiceRefKey)
        {
            var documentInvoice = await oDataService.GetDocument_СчетФактураВыданный(invoiceRefKey);
            if (documentInvoice is null)
            {
                logger.LogError("{Source} Document_СчетФактураВыданный Not Found", nameof(CreateByInvoiceAsync));
                return null;
            }

            var saleDoc = SaleDoc.From(documentInvoice);

            await UpdateCustomer(saleDoc);

            await CreateIfNotExistsAsync(saleDoc);

            return saleDoc;
        }

        public async Task CreateByBaseDocAsync(string mngrOrderString)
        {
            var mngrOrders = JsonSerializer.Deserialize<MngrOrder[]>(mngrOrderString);
            if (mngrOrders is null)
            {
                logger.LogError("{Source} Mngr Orders is null", nameof(CreateByBaseDocAsync));
                return;
            }

            var baseDocs = mngrOrders.Where(e => e.Распоряжение_Name != null && e.Распоряжение_Name.Contains(Document_РеализацияТоваровУслуг.DocumentName));

            foreach (var baseDoc in baseDocs)
            {
                var documentInvoice = await oDataService.GetDocument_СчетФактураВыданный_ByBaseDoc(baseDoc.Распоряжение_Id);
                if (documentInvoice is null)
                {
                    logger.LogError("{Source} Document_СчетФактураВыданный Not Found", nameof(CreateByBaseDocAsync));
                    return;
                }

                var saleDoc = SaleDoc.From(documentInvoice);

                await UpdateCustomer(saleDoc);

                await CreateIfNotExistsAsync(saleDoc);
            }
        }

        private async Task UpdateCustomer(SaleDoc saleDoc)
        {
            if (string.IsNullOrEmpty(saleDoc.CustomerId))
                return;

            var customer = await oDataService.GetCatalog_Контрагенты(saleDoc.CustomerId);

            saleDoc.CustomerName = customer?.Description;
        }

        public async Task UpdateAsync(SaleDoc item)
        {
            item.UserId = await authService.GetCurrentUserIdAsync();

            dbContext.Update(item);

            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateBySubmitAsync(SaleDoc item)
        {
            UpdatePositionWhenSubmit(item);

            dbContext.SaleDocLogs.Add(new SaleDocLog(item));

            await UpdateAsync(item);

            await CreateOrUpdate1cOriginalDocumentReceivedRecord(item);

            item.TaskId = await CreateTask(item);
        }

        private static void UpdatePositionWhenSubmit(SaleDoc saleDoc)
        {
            if (saleDoc.IsCorrect)
            {
                switch (saleDoc.Position.Id)
                {
                    case 1: // Operators
                        saleDoc.PositionId = Position.Accounting.Id;
                        break;
                    case 2: // Managers
                        saleDoc.PositionId = Position.ForDispatch.Id;
                        saleDoc.Redispatch++;
                        break;
                    case 3: // Accounting
                        saleDoc.PositionId = Position.Closed.Id;
                        break;
                    case 4: // Closed
                        saleDoc.PositionId = Position.ForDispatch.Id;
                        break;
                }
            }
            else
            {
                saleDoc.PositionId = Position.Managers.Id;
            }
        }

        private async Task CreateOrUpdate1cOriginalDocumentReceivedRecord(SaleDoc item)
        {
            if (item.Position != Position.Closed || string.IsNullOrEmpty(item.BaseDocId))
                return;

            var record = await oDataService.GetInformationRegister_КОД_ПолученОригиналДокумента(item.BaseDocId);

            if (record == null)
            {
                record = new InformationRegister_КОД_ПолученОригиналДокумента() { Документ_Key = item.BaseDocId, ЕстьДокументы = true };

                await oDataService.PostInformationRegister_КОД_ПолученОригиналДокумента(record);
            }
            else
            {
                record.ЕстьДокументы = true;

                await oDataService.PatchInformationRegister_КОД_ПолученОригиналДокумента(record);
            }
        }

        private async Task<Guid?> CreateTask(SaleDoc item)
        {
            if (item.Position != Position.Managers)
                return null;

            var oneSTask = new OneSTask
            {
                Date = DateTime.Now,
                Исполнитель = item.AuthorId,
                Исполнитель_Type = "StandardODATA.Catalog_Пользователи",
                СрокИсполнения = DateTime.Now.AddDays(1),
                Предмет = item.BaseDocId?.ToString(),
                Предмет_Type = "StandardODATA.Document_РеализацияТоваровУслуг",
                ПредметСтрокой = $"Реализация № {item.BaseDoc?.Number} {item.BaseDoc?.Date}",
                Description = "Переделать документ реализации",
                Описание = $"#DocCheck\n{item.GetErrorDetails()}"
            };

            var result = await oDataService.PostTask(oneSTask);

            if (result.Ref_Key is null)
                return null;

            return Guid.Parse(result.Ref_Key);
        }

        public async Task<List<SaleDoc>> GetListUnclosedAsync(SearchParams searchParams)
        {
            var result = await dbContext.SaleDocs
                .AsNoTracking()
                .Where(e => e.PositionId != Position.Closed.Id)
                .HandleSearch(searchParams)
                .OrderByDescending(e => e.Date)
                .Take(1000)
                .ToListAsync();

            return result;
        }

        public async Task<SaleDoc?> GetAsync(Guid id)
        {
            var item = await dbContext.SaleDocs
                .Include(e => e.PaperworkErrors)
                .Include(e => e.QuantityErrors)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (item?.BaseDocId is null)
                return item;

            var record = await oDataService.GetInformationRegister_КОД_ПолученОригиналДокумента(item.BaseDocId);

            item.IsOriginalDocumentReceived = record is not null && record.ЕстьДокументы;

            return item;
        }

        public async Task<ServiceResult<SaleDoc>> GetByAccessRightsAsync(Guid id)
        {
            var item = await GetAsync(id);

            if (item is null)
                return Error.NotFound;

            if (!(item.Position == Position.ForDispatch || item.Position == Position.Closed) && !await authService.IsCurrentUserInRole(item.Position.Role))
                return Error.AccessDenied;

            if (!string.IsNullOrEmpty(item.BaseDocId))
                item.BaseDoc = await oDataService.GetDocument_РеализацияТоваровУслуг(item.BaseDocId);

            return item;
        }

        public async Task<ServiceResult<SaleDoc>> GetByBarcodeAsync(string barcode)
        {
            var invoiceRefKey = GuidConvert.FromNumStr(barcode);

            if (!Guid.TryParse(invoiceRefKey, out var id))
                return Error.GuidParseFail;

            var item = await GetAsync(id) ?? await CreateByInvoiceAsync(invoiceRefKey);

            if (item is null)
                return Error.NotFound;

            await UpdatePositionWhenOpenByBarcodeAsync(item);

            await UpdateAsync(item);

            if (!string.IsNullOrEmpty(item.BaseDocId))
                item.BaseDoc = await oDataService.GetDocument_РеализацияТоваровУслуг(item.BaseDocId);

            return item;
        }

        public async Task<SaleDoc?> GetByTaskAsync(Guid taskId)
        {
            var result = await dbContext.SaleDocs.FirstOrDefaultAsync(e => e.TaskId == taskId);

            return result;
        }

        private async Task UpdatePositionWhenOpenByBarcodeAsync(SaleDoc item)
        {
            if (item.Position == Position.Closed)
                return;

            if (await authService.IsCurrentUserInRole(Position.Operators.Role))
                item.PositionId = Position.Operators.Id;

            if (await authService.IsCurrentUserInRole(Position.Managers.Role))
                item.PositionId = Position.Managers.Id;

            if (await authService.IsCurrentUserInRole(Position.Accounting.Role))
                item.PositionId = Position.Accounting.Id;
        }

        public async Task DeleteUserAsync(string userId)
        {
            await dbContext.SaleDocs
                .Where(e => e.UserId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var saledoc = await dbContext.SaleDocs.FindAsync(id);
            if (saledoc is null)
                return;

            dbContext.SaleDocs.Remove(saledoc!);

            await dbContext.SaveChangesAsync();
        }

        public async Task ActualizeAsync()
        {
            var items = dbContext.SaleDocs
                .Where(e => e.PositionId != Position.Closed.Id)
                .ToList();

            foreach (var item in items)
            {
                if (item.CustomerId is null)
                {
                    var documentInvoice = await oDataService.GetDocument_СчетФактураВыданный(item.Id.ToString());
                    if (documentInvoice is null || string.IsNullOrEmpty(documentInvoice.Контрагент))
                        return;

                    item.CustomerId = documentInvoice.Контрагент;
                }
                await UpdateCustomer(item);
            }
            dbContext.SaveChanges();
        }

        public string?[]? GetCustomers()
        {
            var result = dbContext.SaleDocs
                .DistinctBy(e => e.CustomerId)
                .Select(e => e.CustomerName)
                .ToArray();

            return result;
        }

        public async Task HandleTaskAsync(string message)
        {
            var oneSTask = JsonSerializer.Deserialize<OneSTask>(message);

            if (oneSTask is null || oneSTask.Ref_Key is null)
                return;

            var taskId = Guid.Parse(oneSTask.Ref_Key);

            var saleDoc = await GetByTaskAsync(taskId);

            if (saleDoc is null)
                return;

            if (oneSTask.Executed)
                saleDoc.PositionId = Position.ForDispatch.Id;
            else
                saleDoc.PositionId = Position.Managers.Id;

            await dbContext.SaveChangesAsync();
        }
    }
}
