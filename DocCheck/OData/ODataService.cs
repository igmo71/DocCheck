using DocCheck.Models.OData;
using DocCheck.Services;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace DocCheck.OData
{
    public class ODataService(ODataClient oDataClient)
    {

        public async ValueTask<Rootobject<TGridItem>?> GetDataAsync<TGridItem>(SearchParams searchParams, ODataParams oDataParams)
        {
            string query = BuildQuery<TGridItem>(searchParams, oDataParams);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<TGridItem>>(query);

            return rootobject;
        }

        public async ValueTask<Rootobject<TGridItem>?> GetDataAsync<TGridItem>(SearchParams searchParams, ODataParams oDataParams, GridItemsProviderRequest<TGridItem> request)
        {
            string query = BuildQuery(searchParams, oDataParams, request);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<TGridItem>>(query);

            return rootobject;
        }

        public async Task<int> GetTotalCountAsync<TGridItem>()
        {
            var result = await oDataClient.GetDataAsync<int>($"{typeof(TGridItem).Name}/$count");
            return result;
        }

        private static string BuildQuery<TGridItem>(SearchParams searchParams, ODataParams oDataParams)
        {
            var query = $"{typeof(TGridItem).Name}?$format=json";

            if (!string.IsNullOrEmpty(oDataParams.Inlinecount))
                query += $"&$inlinecount={oDataParams.Inlinecount}";

            if (!string.IsNullOrEmpty(oDataParams.Select))
                query += $"&$select={oDataParams.Select}";

            if (!string.IsNullOrEmpty(oDataParams.Expand))
                query += $"&$expand={oDataParams.Expand}";

            if (searchParams.HasFilterValue)
            {
                var filter = new List<string>();

                if (searchParams.RefKey != null)
                    filter.Add($"Ref_Key eq guid'{searchParams.RefKey}'");

                if (searchParams.Number != null)
                    filter.Add($"substringof('{searchParams.Number}', Number)");

                if (searchParams.Date != null)
                    filter.Add($"Date ge datetime'{searchParams.Date:s}' and Date lt datetime'{((DateTime)searchParams.Date).AddDays(1):s}'");

                query += "&$filter=" + string.Join(" and ", filter);
            }

            return query;
        }

        private static string BuildQuery<TGridItem>(SearchParams searchParams, ODataParams oDataParams, GridItemsProviderRequest<TGridItem> request)
        {
            var query = BuildQuery<TGridItem>(searchParams, oDataParams) + $"&$skip={request.StartIndex}&$top={request.Count}";

            if (request.SortByColumn != null)
            {
                var sortParam = request.SortByColumn.Title switch
                {
                    "Номер" => "Number",
                    "Дата" => "Date",
                    _ => throw new NotImplementedException()
                };

                query += request.SortByAscending ? $"&$orderby={sortParam}" : $"&$orderby={sortParam} desc";
            }

            return query;
        }
    }
}
