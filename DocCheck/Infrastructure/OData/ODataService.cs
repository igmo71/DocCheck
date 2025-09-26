using DocCheck.Infrastructure.OData.Models;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace DocCheck.Infrastructure.OData
{
    public interface IODataService
    {
        Task<Document_СчетФактураВыданный?> GetDocument_СчетФактураВыданный(string refKey);
        Task<Document_СчетФактураВыданный?> GetDocument_СчетФактураВыданный_ByBaseDoc(string documentSaleRefKey);
        Task<Document_РеализацияТоваровУслуг?> GetDocument_РеализацияТоваровУслуг(string refKey);
        Task<Document_РеализацияТоваровУслуг_Товары[]?> GetDocument_РеализацияТоваровУслуг_Товары(string refKey);

        Task<InformationRegister_КОД_ПолученОригиналДокумента?> GetInformationRegister_КОД_ПолученОригиналДокумента(string docRefKey);
        Task<bool> PostInformationRegister_КОД_ПолученОригиналДокумента(InformationRegister_КОД_ПолученОригиналДокумента value);
        Task<bool> PatchInformationRegister_КОД_ПолученОригиналДокумента(InformationRegister_КОД_ПолученОригиналДокумента value);

        Task<Catalog_Пользователи[]?> GetSalesDepartment_Catalog_Пользователи();
        Task<Catalog_Контрагенты?> GetCatalog_Контрагенты(string refKey);
        Task<Catalog_Контрагенты[]?> GetCatalog_Контрагенты_BySearchTerm(string searchTerm);
        Task<OneSTask> PostTask(OneSTask oneSTask);
    }

    public class ODataService(ODataClient oDataClient) : IODataService
    {

        public async Task<Document_СчетФактураВыданный?> GetDocument_СчетФактураВыданный(string refKey)
        {
            var uri = Document_СчетФактураВыданный.GetUri(refKey);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<Document_СчетФактураВыданный>>(uri);

            var result = rootobject?.Value?.FirstOrDefault();

            return result;
        }

        public async Task<Document_СчетФактураВыданный?> GetDocument_СчетФактураВыданный_ByBaseDoc(string documentSaleRefKey)
        {
            var uri = Document_СчетФактураВыданный.GetUriByBaseDoc(documentSaleRefKey);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<Document_СчетФактураВыданный>>(uri);

            var result = rootobject?.Value?.FirstOrDefault();

            return result;
        }

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

        public async Task<InformationRegister_КОД_ПолученОригиналДокумента?> GetInformationRegister_КОД_ПолученОригиналДокумента(string docRefKey)
        {
            var uri = InformationRegister_КОД_ПолученОригиналДокумента.GetUri(docRefKey);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<InformationRegister_КОД_ПолученОригиналДокумента>>(uri);

            var result = rootobject?.Value?.FirstOrDefault();

            return result;
        }

        public async Task<bool> PostInformationRegister_КОД_ПолученОригиналДокумента(InformationRegister_КОД_ПолученОригиналДокумента value)
        {
            if (string.IsNullOrEmpty(value.Документ_Key))
                return false;

            var uri = InformationRegister_КОД_ПолученОригиналДокумента.PostUri();

            var result = await oDataClient.PostDataAsPostmanAsync(uri, value);

            return result;
        }

        public async Task<bool> PatchInformationRegister_КОД_ПолученОригиналДокумента(InformationRegister_КОД_ПолученОригиналДокумента value)
        {
            if (string.IsNullOrEmpty(value.Документ_Key))
                return false;

            var uri = InformationRegister_КОД_ПолученОригиналДокумента.PatchUri(value.Документ_Key);

            var result = await oDataClient.PatchDataAsPostmanAsync(uri, value);

            return result;
        }

        public async Task<Catalog_Пользователи[]?> GetSalesDepartment_Catalog_Пользователи()
        {
            var uri = Catalog_Пользователи.GetSalesDepartmentUri();

            var rootobject = await oDataClient.GetDataAsync<Rootobject<Catalog_Пользователи>>(uri);

            var result = rootobject?.Value;

            return result;
        }

        public async Task<Catalog_Контрагенты?> GetCatalog_Контрагенты(string refKey)
        {
            var uri = Catalog_Контрагенты.GetUri(refKey);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<Catalog_Контрагенты>>(uri);

            var result = rootobject?.Value?.FirstOrDefault();

            return result;
        }

        public async Task<Catalog_Контрагенты[]?> GetCatalog_Контрагенты_BySearchTerm(string searchTerm)
        {
            var uri = Catalog_Контрагенты.GetUriBySearchTerm(searchTerm);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<Catalog_Контрагенты>>(uri);

            var result = rootobject?.Value;

            return result;
        }

        public async Task<OneSTask?> PostTask(OneSTask value)
        {
            var uri = OneSTask.PostUri();

            var response = await oDataClient.PostDataWithResponseAsPostmanAsync(uri, value);

            var result = JsonSerializer.Deserialize<OneSTask>(response);

            return result;
        }
    }
}
