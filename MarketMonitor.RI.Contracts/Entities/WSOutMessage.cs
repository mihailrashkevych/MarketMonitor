using System.Text.Json.Serialization;

namespace MarketMonitor.RI.Contracts.Entities
{
    public record WSOutMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("instrumentId")]
        public Guid InstrumentId { get; set; }

        [JsonPropertyName("provider")]
        public string Provider { get; set; }

        [JsonPropertyName("subscribe")]
        public bool Subscribe { get; set; }

        [JsonPropertyName("kinds")]
        public IEnumerable<string> Kinds { get; set; }
    }
}
