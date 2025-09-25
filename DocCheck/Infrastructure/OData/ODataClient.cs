using DocCheck.Components.Pages;
using System.Text;
using System.Text.Json;

namespace DocCheck.Infrastructure.OData
{
    public class ODataClient(HttpClient httpClient, ILogger<ODataClient> logger)
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // чтобы не было \u0414
            PropertyNamingPolicy = null, // имена свойств остаются как в C#
            WriteIndented = false
        };

        public async Task<TData?> GetDataAsync<TData>(string uri)
        {
            var result = await httpClient.GetFromJsonAsync<TData>(uri);

            logger.LogDebug("{Source} {Uri} {@Result}", nameof(GetDataAsync), uri, result);

            return result;
        }

        public async Task<bool> PostDataAsPostmanAsync<TData>(string uri, TData value)
        {
            var jsonString = JsonSerializer.Serialize(value, jsonSerializerOptions);

            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);            

            await LogResponse(nameof(PostDataAsPostmanAsync), uri, value, response);

            return response.IsSuccessStatusCode;
        }
        
        public async Task<bool> PatchDataAsPostmanAsync<TData>(string uri, TData value)
        {
            var jsonString = JsonSerializer.Serialize(value, jsonSerializerOptions);

            var request = new HttpRequestMessage(HttpMethod.Patch, uri)
            {
                Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            await LogResponse(nameof(PatchDataAsPostmanAsync), uri, value, response);

            return response.IsSuccessStatusCode;
        }

        private async Task LogResponse<TData>(string source, string uri, TData value, HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                logger.LogDebug("{Source} {StatusCode} {Uri} {@Value} {ResponseContent}", source, response.StatusCode, uri, value, responseContent);
            else
                logger.LogError("{Source} {StatusCode} {Uri} {@Value} {ResponseContent}", source, response.StatusCode, uri, value, responseContent);
        }
    }
}
