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
            var query = $"{typeof(TGridItem).Name}?$format=json";

            if(searchParams.IsInlinecount) 
                query += "&$inlinecount=allpages";

            if (!string.IsNullOrEmpty(searchParams.Select))
                query += $"&$select={searchParams.Select}";

            if (!string.IsNullOrEmpty(searchParams.Expand))
                query += $"&$expand={searchParams.Expand}";

            if (searchParams.HasFilterValue)
            {
                var filter = new List<string>();

                if (searchParams.Ref_Key != null)
                    filter.Add($"Ref_Key eq guid'{searchParams.Ref_Key}'");

                if (searchParams.Number != null)
                    filter.Add($"substringof('{searchParams.Number}', Number)");

                if (searchParams.Date != null)
                    filter.Add($"Date ge datetime'{searchParams.Date:s}' and Date lt datetime'{((DateTime)searchParams.Date).AddDays(1):s}'");

                query += "&$filter=" + string.Join(" and ", filter);
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
