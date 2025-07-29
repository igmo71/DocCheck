using DocCheck.Models;
using DocCheck.Models.OData;

namespace DocCheck.Services
{
    public interface IDocCheckService
    {
        Task<DocumentCheck[]> GetItemsAsync(SearchParams searchParams);
        Task<DocumentCheck?> GetItemAsync(string invoiceRefKey, bool isIncludeErrors = false);
        Task LoadBaseDocuments(DocumentCheck documentCheck);
    }

    public class DocCheckService(
        DocCheckRepository repository,
        AuthService authService,
        IInvoiceService invoiceService,
        ISaleDocService saleDocService,
        ICorrectionDocService correctionDocService) : IDocCheckService
    {
        public async Task<DocumentCheck[]> GetItemsAsync(SearchParams searchParams)
        {
            var items = await repository.GetItemsAsync(searchParams);

            foreach (var item in items)
                item.User = await authService.FindByIdAsync(item.UserId);

            return items;
        }

        public async Task<DocumentCheck?> GetItemAsync(string invoiceRefKey, bool isIncludeErrors = false)
        {
            var item = await repository.GetItemAsync(invoiceRefKey, isIncludeErrors);

            if (item is not null)
                item.User = await authService.FindByIdAsync(item.UserId);

            return item;
        }

        public async Task LoadBaseDocuments(DocumentCheck documentCheck)
        {
            if (documentCheck.InvoiceRefKey is null)
                return;

            var invoice = await invoiceService.GetItem(documentCheck.InvoiceRefKey);

            if (invoice is null)
                return;

            documentCheck.InvoiceNumber = invoice.Number;
            documentCheck.InvoiceDate = invoice.Date;

            if (invoice.ДокументыОснования is null)
                return;

            documentCheck.InvoiceBaseDocuments = invoice.ДокументыОснования;

            foreach (var baseDoc in invoice.ДокументыОснования)
            {
                if (!string.IsNullOrEmpty(baseDoc.ДокументОснование_Type))
                {
                    if (baseDoc.ДокументОснование_Type.Contains(nameof(Document_РеализацияТоваровУслуг)))
                    {
                        documentCheck.SaleDocuments ??= [];

                        if (await saleDocService.GetItem(baseDoc.ДокументОснование) is Document_РеализацияТоваровУслуг saleDoc)
                            documentCheck.SaleDocuments.Add(saleDoc);
                    }
                    if (baseDoc.ДокументОснование_Type.Contains(nameof(Document_КорректировкаРеализации)))
                    {
                        documentCheck.CorrectionDocuments ??= [];

                        if (await correctionDocService.GetItem(baseDoc.ДокументОснование) is Document_КорректировкаРеализации correctionDoc)
                            documentCheck.CorrectionDocuments.Add(correctionDoc);
                    }
                }
            }

        }
    }
}
