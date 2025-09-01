using System.Text.Json.Serialization;

namespace DocCheck.Infrastructure.OData.Models
{
    public class Rootobject<TValue>
    {
        [JsonPropertyName("odata.metadata")]
        public string? ODataMetadata { get; set; }


        [JsonPropertyName("odata.count")]
        public int ODataCount { get; set; }


        [JsonPropertyName("value")]
        public TValue[]? Value { get; set; }
    }
}
