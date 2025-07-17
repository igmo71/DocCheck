namespace DocCheck.OData
{
    public class ODataClient(HttpClient httpClient)
    {
        public async Task<TData?> GetDataAsync<TData>(string uri)
        {
            var result = await httpClient.GetFromJsonAsync<TData>(uri);

            return result;
        }
    }
}
