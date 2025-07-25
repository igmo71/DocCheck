using DocCheck.Models.OData;
using DocCheck.OData;

namespace DocCheck.Services
{
    public interface IInvoiceService
    {
        Task<Document_СчетФактураВыданный?> GetItem(string refKey);
    }

    public class InvoiceService(ODataClient oDataClient) : IInvoiceService
    {
        public async Task<Document_СчетФактураВыданный?> GetItem(string refKey)
        {
            string query = BuildQuery(refKey);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<Document_СчетФактураВыданный>>(query);

            var invoice = rootobject?.Value?[0];

            return invoice;
        }

        private string BuildQuery(string refKey)
        {
            var oDataParams = Document_СчетФактураВыданный.ODataParams;

            var query = $"{nameof(Document_СчетФактураВыданный)}" +
                $"?$format=json" +
                $"&$expand={oDataParams.Expand}" +
                $"&$select={oDataParams.Select}" +
                $"&$filter=Ref_Key eq guid'{refKey}'";

            return query;
        }
    }
}