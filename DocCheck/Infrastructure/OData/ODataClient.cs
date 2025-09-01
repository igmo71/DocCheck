namespace DocCheck.Infrastructure.OData
{
    public class ODataClient(HttpClient httpClient, ILogger<ODataClient> logger)
    {
        public async Task<TData?> GetDataAsync<TData>(string uri)
        {
            var result = await httpClient.GetFromJsonAsync<TData>(uri);

            logger.LogDebug("{Source} {Uri} {@Result}", nameof(GetDataAsync), uri, result);

            return result;
        }
    }
}
