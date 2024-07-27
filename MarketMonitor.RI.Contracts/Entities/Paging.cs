using System.Text.Json.Serialization;

namespace MarketMonitor.RI.Contracts.Entities
{
    public record Paging
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("items")]
        public int Items { get; set; }
    }
}
