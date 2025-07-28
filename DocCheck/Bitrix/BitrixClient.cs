using System.Text.Json;

namespace DocCheck.Bitrix
{
    public class BitrixClient(HttpClient httpClient)
    {
        public async Task<TResponse?> PostDataAsync<TRequest, TResponse>(string? uri, TRequest body)
        {
            var response = await httpClient.PostAsJsonAsync(uri, body);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TResponse>(content);

            return result;
        }

        public async Task<TResponse?> PostDataAsync<TResponse>(string? uri, HttpContent httpContent)
        {
            var response = await httpClient.PostAsync(uri, httpContent);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TResponse>(content);

            return result;
        }
    }
}
