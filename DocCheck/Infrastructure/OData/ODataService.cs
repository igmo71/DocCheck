using DocCheck.Infrastructure.OData.Models;

namespace DocCheck.Infrastructure.OData
{
    public interface IODataService
    {
        Task<Document_РеализацияТоваровУслуг?> GetDocument_РеализацияТоваровУслуг(string refKey);
        Task<Document_РеализацияТоваровУслуг_Товары[]?> GetDocument_РеализацияТоваровУслуг_Товары(string refKey);
        Task<Document_СчетФактураВыданный?> GetDocument_СчетФактураВыданный(string refKey);
        Task<Document_СчетФактураВыданный?> GetDocument_СчетФактураВыданный_ByBaseDoc(string documentSaleRefKey);

        //Task<Document_СчетФактураВыданный_ДокументыОснования[]?> GetListDocument_СчетФактураВыданный_ДокументыОснования(string? refKey);
    }

    public class ODataService(ODataClient oDataClient) : IODataService
    {
        public async Task<Document_РеализацияТоваровУслуг?> GetDocument_РеализацияТоваровУслуг(string refKey)
        {
            var uri = Document_РеализацияТоваровУслуг.GetUri(refKey);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<Document_РеализацияТоваровУслуг>>(uri);

            var result = rootobject?.Value?.FirstOrDefault();

            return result;
        }

        public async Task<Document_РеализацияТоваровУслуг_Товары[]?> GetDocument_РеализацияТоваровУслуг_Товары(string refKey)
        {
            var uri = Document_РеализацияТоваровУслуг_Товары.GetUri(refKey);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<Document_РеализацияТоваровУслуг_Товары>>(uri);

            var result = rootobject?.Value;

            return result;
        }

        public async Task<Document_СчетФактураВыданный?> GetDocument_СчетФактураВыданный(string refKey)
        {
            var uri = Document_СчетФактураВыданный.GetUri(refKey);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<Document_СчетФактураВыданный>>(uri);

            var result = rootobject?.Value?.FirstOrDefault();

            return result;
        }

        public async Task<Document_СчетФактураВыданный?> GetDocument_СчетФактураВыданный_ByBaseDoc(string documentSaleRefKey)
        {
            var uri = Document_СчетФактураВыданный.GetUriByDocumentSale(documentSaleRefKey);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<Document_СчетФактураВыданный>>(uri);

            var result = rootobject?.Value?.FirstOrDefault();

            return result;
        }

        //public async Task<Document_СчетФактураВыданный_ДокументыОснования[]?> GetListDocument_СчетФактураВыданный_ДокументыОснования(string? refKey)
        //{
        //    if (refKey is null)
        //        return null;

        //    var uri = Document_СчетФактураВыданный_ДокументыОснования.GetUri(refKey);

        //    var rootobject = await oDataClient.GetDataAsync<Rootobject<Document_СчетФактураВыданный_ДокументыОснования>>(uri);

        //    var result = rootobject?.Value;

        //    return result;
        //}
    }
}
