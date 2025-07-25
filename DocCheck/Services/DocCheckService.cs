using DocCheck.Models;

namespace DocCheck.Services
{
    public interface IDocCheckService
    {
        Task<DocumentCheck> GetItem(string invoiceRefKey);
        Task<List<DocumentCheck>> GetItems(SearchParams SearchParams);
    }

    public class DocCheckService(
        DocCheckRepository repository,
        IInvoiceService invoiceService,
        SaleDocumentService saleDocService,
        CorrectionDocumentService correctionDocService) : IDocCheckService
    {
        public Task<DocumentCheck> GetItem(string invoiceRefKey)
        {
            throw new NotImplementedException();
        }

        public Task<List<DocumentCheck>> GetItems(SearchParams SearchParams)
        {
            throw new NotImplementedException();
        }
    }
}
