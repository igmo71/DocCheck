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
        Task CreateByBaseDocAsync(string mngrOrderString);
        Task UpdateBySubmitAsync(SaleDoc saleDoc);
        Task<List<SaleDoc>> GetListUnclosedAsync(SaleDocSearchParams searchParams);
        Task<ServiceResult<SaleDoc>> GetByAccessRightsAsync(Guid id);
        Task<ServiceResult<SaleDoc>> GetByBarcodeAsync(string barcode);
        Task<SaleDoc?> GetAsync(Guid id);
        Task DeleteUserAsync(string userId);
        Task DeleteAsync(Guid id);
        Task HandleManagerTaskAsync(string message);
        Task ActualizeAsync();
    }

    public class SaleDocService(
       ApplicationDbContext dbContext,
       IODataService oDataService,
       AuthService authService,
       IConfiguration configuration,
       ILogger<SaleDocService> logger) : ISaleDocService
    {
        public async Task CreateByBaseDocAsync(string mngrOrderString)
        {
            var source = nameof(CreateByBaseDocAsync);

            logger.LogDebug("{Source} Start {MngrOrderString}", source, mngrOrderString);

            var mngrOrders = JsonSerializer.Deserialize<MngrOrder[]>(mngrOrderString);

            if (mngrOrders is null)
            {
                logger.LogError("{Source} Mngr Orders is null", source);
                return;
            }

            var baseDocs = mngrOrders.Where(e => e.Распоряжение_Name != null && e.Распоряжение_Name.Contains(Document_РеализацияТоваровУслуг.DocumentName));

            foreach (var baseDoc in baseDocs)
            {
                var documentInvoice = await oDataService.GetDocument_СчетФактураВыданный(baseDoc.Распоряжение_Id, Document_СчетФактураВыданный.GetBy.BaseDoc);

                var saleDoc = await BuildByInvoice(documentInvoice);

                if (saleDoc is null)
                    logger.LogError("{Source} Failed to build a SaleDoc by baseDoc ({Распоряжение_Id})", source, baseDoc.Распоряжение_Id);
                else
                    await CreateIfNotExistsAsync(saleDoc);
            }

            logger.LogDebug("{Source} Ok {MngrOrderString}", source, mngrOrderString);
        }

        private async Task<SaleDoc?> BuildByInvoice(Document_СчетФактураВыданный? documentInvoice)
        {
            logger.LogDebug("{Source} Start {@DocumentInvoice}", nameof(BuildByInvoice), documentInvoice);

            if (documentInvoice is null)
            {
                logger.LogError("{Source} Document_СчетФактураВыданный Not Found", nameof(BuildByInvoice));
                return null;
            }

            if (documentInvoice.ДокументОснование is null)
            {
                logger.LogError("{Source} ДокументОснование Is Null", nameof(BuildByInvoice));
                return null;
            }

            var documentBbase = await oDataService.GetDocument_РеализацияТоваровУслуг(documentInvoice.ДокументОснование);

            if (documentBbase is null)
            {
                logger.LogError("{Source} Document_РеализацияТоваровУслуг Not Found", nameof(BuildByInvoice));
                return null;
            }

            var saleDoc = SaleDoc.From(documentInvoice, documentBbase);

            logger.LogDebug("{Source} Ok {@DocumentInvoice} {@SaleDoc}", nameof(BuildByInvoice), documentInvoice, saleDoc);

            return saleDoc;
        }

        private async Task CreateIfNotExistsAsync(SaleDoc saleDoc)
        {
            var source = nameof(CreateIfNotExistsAsync);

            logger.LogDebug("{Source} Try {@SaleDoc}", source, saleDoc);

            if (dbContext.SaleDocs.Any(e => e.Id == saleDoc.Id))
            {
                logger.LogDebug("{Source} Exists {@SaleDoc}", source, saleDoc);
                return;
            }

            saleDoc.PositionId = Position.ForDispatch.Id;

            await dbContext.SaleDocs.AddAsync(saleDoc);

            await dbContext.SaveChangesAsync();

            logger.LogDebug("{Source} Ok {@SaleDoc}", source, saleDoc);
        }

        public async Task UpdateBySubmitAsync(SaleDoc item)
        {
            logger.LogDebug("{Source} Start {@SaleDoc}", nameof(UpdateBySubmitAsync), item);

            UpdatePositionWhenSubmit(item);

            dbContext.SaleDocLogs.Add(new SaleDocLog(item));

            item.ManagerTaskId = await CreateManagerTask(item);

            await UpdateAsync(item);

            await CreateOrUpdate1cOriginalDocumentReceivedRecord(item);

            logger.LogDebug("{Source} Ok {@SaleDoc}", nameof(UpdateBySubmitAsync), item);
        }

        private void UpdatePositionWhenSubmit(SaleDoc saleDoc)
        {
            logger.LogDebug("{Source} From {Position}", nameof(UpdatePositionWhenSubmit), saleDoc.Position.Description);

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

            logger.LogDebug("{Source} To {Position}", nameof(UpdatePositionWhenSubmit), saleDoc.Position.Description);
        }

        private async Task CreateOrUpdate1cOriginalDocumentReceivedRecord(SaleDoc item)
        {
            logger.LogDebug("{Source} Try {@SaleDoc}", nameof(CreateOrUpdate1cOriginalDocumentReceivedRecord), item);

            if (item.Position != Position.Closed || string.IsNullOrEmpty(item.BaseDocId))
                return;

            logger.LogDebug("{Source} Start {@SaleDoc}", nameof(CreateOrUpdate1cOriginalDocumentReceivedRecord), item);

            var record = await oDataService.GetInformationRegister_КОД_ПолученОригиналДокумента(item.BaseDocId);

            if (record == null)
            {
                record = new InformationRegister_КОД_ПолученОригиналДокумента() { Документ_Key = item.BaseDocId, ЕстьДокументы = true };

                var isSuccess = await oDataService.PostInformationRegister_КОД_ПолученОригиналДокумента(record);

                if (isSuccess)
                    logger.LogDebug("{Source} Posted {@SaleDoc}", nameof(CreateOrUpdate1cOriginalDocumentReceivedRecord), item);
            }
            else
            {
                record.ЕстьДокументы = true;

                var isSuccess = await oDataService.PatchInformationRegister_КОД_ПолученОригиналДокумента(record);

                if (isSuccess)
                    logger.LogDebug("{Source} Patched {@SaleDoc}", nameof(CreateOrUpdate1cOriginalDocumentReceivedRecord), item);
            }
        }

        private async Task<Guid?> CreateManagerTask(SaleDoc item)
        {
            logger.LogDebug("{Source} Try {@SaleDoc}", nameof(CreateManagerTask), item);

            if (item.Position != Position.Managers)
                return null;

            logger.LogDebug("{Source} Start {@SaleDoc}", nameof(CreateManagerTask), item);

            var taskHandlerConfig = configuration["TaskHandler"];

            if (taskHandlerConfig is null)
            {
                logger.LogError("{Source} TaskHandler Config Not Found", nameof(CreateManagerTask));
                return null;
            }

            var taskHandlerId = item.GetTaskHandler(taskHandlerConfig);

            if (taskHandlerId is null)
            {
                logger.LogError("{Source} TaskHandler Id Not Found", nameof(CreateManagerTask));
                return null;
            }

            if (item.BaseDoc is null && !string.IsNullOrEmpty(item.BaseDocId))
                item.BaseDoc = await oDataService.GetDocument_РеализацияТоваровУслуг(item.BaseDocId);

            var oneSTask = new OneSTask
            {
                Date = DateTime.Now,
                Исполнитель = taskHandlerId,
                Исполнитель_Type = "StandardODATA.Catalog_Пользователи",
                СрокИсполнения = DateTime.Now.AddDays(1),
                Предмет = item.BaseDocId?.ToString(),
                Предмет_Type = "StandardODATA.Document_РеализацияТоваровУслуг",
                ПредметСтрокой = $"Реализация № {item.BaseDoc?.Number} от {item.BaseDoc?.Date}",
                Description = "Переделать документ реализации",
                Описание = $"#DocCheck\n{item.GetErrorDetails()}"
            };

            var result = await oDataService.CreateTask(oneSTask);

            logger.LogDebug("{Source} Ok {@PostedTask} {@Response}", nameof(CreateManagerTask), oneSTask, result);

            if (result is null || result.Ref_Key is null)
                return null;

            return Guid.Parse(result.Ref_Key);
        }

        public async Task<List<SaleDoc>> GetListUnclosedAsync(SaleDocSearchParams searchParams)
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
            var source = nameof(GetByBarcodeAsync);

            logger.LogDebug("{Source} Start {Barcode}", source, barcode);

            var invoiceRefKey = GuidConvert.FromNumStr(barcode);

            logger.LogDebug("{Source} GuidConvert.FromNumStr {Barcode} -> {InvoiceRefKey}", source, barcode, invoiceRefKey);

            if (!Guid.TryParse(invoiceRefKey, out var id))
            {
                logger.LogError("{Source} Error Parse {InvoiceRefKey}", source, invoiceRefKey);
                return Error.GuidParseFail;
            }

            var item = await GetAsync(id);

            if (item is null)
            {
                logger.LogDebug("{Source} Not Found By InvoiceKey ({InvoiceRefKey}). Try Create. {Barcode}", source, invoiceRefKey, barcode);

                item = await CreateByInvoiceKeyAsync(invoiceRefKey);

                if (item is null)
                {
                    logger.LogError("{Source} Error Create By InvoiceKey ({InvoiceRefKey}) {Barcode}", source, invoiceRefKey, barcode);
                    return Error.NotFound;
                }
            }

            await UpdatePositionWhenOpenByBarcodeAsync(item);

            await UpdateAsync(item);

            if (!string.IsNullOrEmpty(item.BaseDocId))
                item.BaseDoc = await oDataService.GetDocument_РеализацияТоваровУслуг(item.BaseDocId);

            logger.LogDebug("{Source} Ok {Barcode}", source, barcode);

            return item;
        }

        private async Task<SaleDoc?> CreateByInvoiceKeyAsync(string invoiceRefKey)
        {
            var source = nameof(CreateByInvoiceKeyAsync);

            logger.LogDebug("{Source} Start {InvoiceRefKey}", source, invoiceRefKey);

            var documentInvoice = await oDataService.GetDocument_СчетФактураВыданный(invoiceRefKey, Document_СчетФактураВыданный.GetBy.RefKey);

            var saleDoc = await BuildByInvoice(documentInvoice);

            if (saleDoc is null)
            {
                logger.LogError("{Source} Failed to build a SaleDoc by invoiceRefKey ({invoiceRefKey})", source, invoiceRefKey);
                return null;
            }

            await CreateIfNotExistsAsync(saleDoc);

            logger.LogDebug("{Source} Ok {InvoiceRefKey} {@SaleDoc}", source, invoiceRefKey, saleDoc);

            return saleDoc;
        }

        private async Task UpdatePositionWhenOpenByBarcodeAsync(SaleDoc item)
        {
            logger.LogDebug("{Source} From {Position}", nameof(UpdatePositionWhenOpenByBarcodeAsync), item.Position.Description);

            if (item.Position == Position.Closed)
                return;

            if (await authService.IsCurrentUserInRole(Position.Operators.Role))
                item.PositionId = Position.Operators.Id;

            if (await authService.IsCurrentUserInRole(Position.Managers.Role))
                item.PositionId = Position.Managers.Id;

            if (await authService.IsCurrentUserInRole(Position.Accounting.Role))
                item.PositionId = Position.Accounting.Id;

            logger.LogDebug("{Source} To {Position}", nameof(UpdatePositionWhenOpenByBarcodeAsync), item.Position.Description);
        }

        private async Task UpdateAsync(SaleDoc item)
        {
            item.UserId = await authService.GetCurrentUserIdAsync();

            dbContext.Update(item);

            await dbContext.SaveChangesAsync();
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

        public async Task HandleManagerTaskAsync(string message)
        {
            logger.LogDebug("{Source} Start {Message}", nameof(HandleManagerTaskAsync), message);

            var inputTask = JsonSerializer.Deserialize<OneSTask>(message);

            if (inputTask?.Ref_Key is null)
            {
                logger.LogDebug("{Source} Error Deserialize of InputTask {InputTask}", nameof(HandleManagerTaskAsync), inputTask);
                return;
            }

            var taskId = Guid.Parse(inputTask.Ref_Key);

            var saleDoc = await dbContext.SaleDocs.FirstOrDefaultAsync(e => e.ManagerTaskId == taskId);

            if (saleDoc is null)
            {
                logger.LogDebug("{Source} SaleDoc Not Found by TaskId ({TaskId})", nameof(HandleManagerTaskAsync), taskId);
                return;
            }

            var oneSTask = await oDataService.GetTask(inputTask.Ref_Key);

            if (oneSTask?.Ref_Key is null)
            {
                logger.LogDebug("{Source} OneSTask Not Found by TaskId ({TaskId})", nameof(HandleManagerTaskAsync), oneSTask?.Ref_Key);
                return;
            }

            logger.LogDebug("{Source} From {Position}  {oneSTask}", nameof(HandleManagerTaskAsync), saleDoc.Position.Description, oneSTask);

            if (oneSTask.Executed)
            {
                saleDoc.PositionId = Position.ForDispatch.Id;
                saleDoc.Redispatch++;
            }
            else
                saleDoc.PositionId = Position.Managers.Id;

            await dbContext.SaveChangesAsync();

            logger.LogDebug("{Source} To {Position}  {oneSTask}", nameof(HandleManagerTaskAsync), saleDoc.Position.Description, oneSTask);
        }

        public async Task ActualizeAsync()
        {
            logger.LogInformation("{Source} Start", nameof(ActualizeAsync));

            var items = dbContext.SaleDocs
                .Include(e => e.PaperworkErrors)
                .Include(e => e.QuantityErrors)
                .Where(e => e.PositionId != Position.Closed.Id)
                .ToList();

            foreach (var item in items)
            {
                var documentInvoice = await oDataService.GetDocument_СчетФактураВыданный(item.Id.ToString(), Document_СчетФактураВыданный.GetBy.RefKey);

                if (documentInvoice is null)
                {
                    logger.LogWarning("{Source} Document_СчетФактураВыданный Not Found by RefKey ({RefKey})", nameof(ActualizeAsync), item.Id.ToString());
                    continue;
                }

                var actualItem = await BuildByInvoice(documentInvoice);

                if (actualItem is null)
                {
                    logger.LogWarning("{Source} Failed to build a SaleDoc by invoiceRefKey ({invoiceRefKey})", nameof(ActualizeAsync), documentInvoice);
                    continue;
                }

                item.AuthorId = actualItem.AuthorId;
                item.AuthorName = actualItem.AuthorName;
                item.CustomerId = actualItem.CustomerId;
                item.CustomerName = actualItem.CustomerName;
                item.ManagerId = actualItem.ManagerId;
                item.ManagerName = actualItem.ManagerName;

                if (item.Position == Position.Managers && item.ManagerTaskId is null)
                    item.ManagerTaskId = await CreateManagerTask(item);

                await dbContext.SaveChangesAsync();
            }

            logger.LogInformation("{Source} Ok", nameof(ActualizeAsync));
        }
    }
}
