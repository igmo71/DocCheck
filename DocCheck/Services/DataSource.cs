using DocCheck.Models.OData;
using DocCheck.OData;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace DocCheck.Services
{
    public class DataSource(ODataClient oDataClient)
    {

        public async ValueTask<Rootobject<TGridItem>?> GetDataAsync<TGridItem>(SearchParams searchParams)
        {
            string query = BuildQuery<TGridItem>(searchParams);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<TGridItem>>(query);

            return rootobject;
        }

        public async ValueTask<Rootobject<TGridItem>?> GetDataAsync<TGridItem>(SearchParams searchParams, GridItemsProviderRequest<TGridItem> request)
        {
            string query = BuildQuery<TGridItem>(searchParams, request);

            var rootobject = await oDataClient.GetDataAsync<Rootobject<TGridItem>>(query);

            return rootobject;
        }

        public async Task<int> GetTotalCountAsync<TGridItem>()
        {
            var result = await oDataClient.GetDataAsync<int>($"{typeof(TGridItem).Name}/$count");
            return result;
        }

        private static string BuildQuery<TGridItem>(SearchParams searchParams)
        {
            var query = $"{typeof(TGridItem).Name}?$format=json&$inlinecount=allpages&$select=Ref_Key,Number,Date";

            if (searchParams.HasAnyValue)
            {
                var searchList = new List<string>();

                if (searchParams.Ref_Key != null)
                    searchList.Add($"Ref_Key eq guid'{searchParams.Ref_Key}'");

                if (searchParams.Number != null)
                    searchList.Add($"substringof('{searchParams.Number}', Number)");

                if (searchParams.Date != null)
                    searchList.Add($"Date ge datetime'{searchParams.Date:s}' and Date lt datetime'{((DateTime)searchParams.Date).AddDays(1):s}'");

                query += "&$filter=" + string.Join(" and ", searchList);
            }

            return query;
        }

        private static string BuildQuery<TGridItem>(SearchParams searchParams, GridItemsProviderRequest<TGridItem> request)
        {
            var query = BuildQuery<TGridItem>(searchParams) + $"&$skip={request.StartIndex}&$top={request.Count}";

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
