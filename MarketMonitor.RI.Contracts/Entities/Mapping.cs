using System.Text.Json.Serialization;

namespace MarketMonitor.RI.Contracts.Entities
{
    public record Mapping
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("defaultOrderSize")]
        public int DefaultOrderSize { get; set; }
    }
}