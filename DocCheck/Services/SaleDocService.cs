using DocCheck.Models.OData;
using DocCheck.OData;

namespace DocCheck.Services
{
    public interface ISaleDocService
    {
        Task<Document_РеализацияТоваровУслуг?> GetItem(string? refKey);
    }

    public class SaleDocService(ODataClient oDataClient) : ISaleDocService
    {
        public async Task<Document_РеализацияТоваровУслуг?> GetItem(string? refKey)
        {
            string query = BuildQuery(refKey);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<Document_РеализацияТоваровУслуг>>(query);

            var saleDoc = rootobject?.Value?.FirstOrDefault();

            if(saleDoc == null) 
                return null;    

            saleDoc.Товары = await GetProducts(saleDoc.Ref_Key);

            return saleDoc;
        }

        private string BuildQuery(string? refKey)
        {
            var oDataParams = Document_РеализацияТоваровУслуг.ODataParams;

            var query = $"{nameof(Document_РеализацияТоваровУслуг)}" +
                $"?$format=json" +
                $"&$filter=Ref_Key eq guid'{refKey}'" +
                $"&$expand={oDataParams.Expand}" +
                $"&$select={oDataParams.Select}";

            return query;
        }

        private async Task<Document_РеализацияТоваровУслуг_Товары[]?> GetProducts(string? refKey)
        {
            string query = BuildProductsQuery(refKey);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<Document_РеализацияТоваровУслуг_Товары>>(query);

            var products = rootobject?.Value;

            return products;
        }

        private string BuildProductsQuery(string? refKey)
        {
            var oDataParams = Document_РеализацияТоваровУслуг_Товары.ODataParams;

            var query = $"{nameof(Document_РеализацияТоваровУслуг_Товары)}" +
                $"?$format=json" +
                $"&$filter=Ref_Key eq guid'{refKey}'" +
                $"&$expand={oDataParams.Expand}" +
                $"&$select={oDataParams.Select}" +
                $"&$orderby={oDataParams.OrderBy}";

            return query;
        }
    }
}