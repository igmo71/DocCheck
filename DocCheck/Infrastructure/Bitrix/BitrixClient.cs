using System.Text.Json;

namespace DocCheck.Infrastructure.Bitrix
{
    public class BitrixClient(HttpClient httpClient, ILogger<BitrixClient> logger)
    {
        public async Task<TResponse?> PostDataAsync<TResponse>(string? uri, HttpContent httpContent)
        {
            var response = await httpClient.PostAsync(uri, httpContent);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TResponse>(content);

            logger.LogDebug("{Source} {Uri} {@Result}", nameof(PostDataAsync), uri, result);

            return result;
        }
    }
}
