using System.Text.Json.Serialization;

namespace MarketMonitor.RI.Contracts.Entities
{
    public record Asset
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("tickSize")]
        public decimal TickSize { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("baseCurrency")]
        public string? BaseCurrency { get; set; }

        [JsonPropertyName("mappings")]
        public Dictionary<string, Mapping>? Mappings { get; set; }
    }
}
