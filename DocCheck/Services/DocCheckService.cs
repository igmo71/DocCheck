using DocCheck.Models;
using DocCheck.Models.OData;

namespace DocCheck.Services
{
    public interface IDocCheckService
    {
        //Task<DocumentCheck> GetItem(string invoiceRefKey);
        //Task<List<DocumentCheck>> GetItems(SearchParams SearchParams);
        Task LoadBaseDocuments(DocumentCheck documentCheck);
    }

    public class DocCheckService(
        DocCheckRepository repository,
        IInvoiceService invoiceService,
        ISaleDocService saleDocService,
        ICorrectionDocService correctionDocService) : IDocCheckService
    {
        //public Task<DocumentCheck> GetItem(string invoiceRefKey)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<List<DocumentCheck>> GetItems(SearchParams SearchParams)
        //{
        //    throw new NotImplementedException();
        //}

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
