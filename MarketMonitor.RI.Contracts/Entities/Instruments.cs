using System.Text.Json.Serialization;

namespace MarketMonitor.RI.Contracts.Entities
{
    public record Instruments
    {
        [JsonPropertyName("paging")]
        public Paging? Paging { get; set; }

        [JsonPropertyName("data")]
        public IEnumerable<Asset>? Assets  { get; set; }
    }
}
