using System.Text.Json.Serialization;

namespace MarketMonitor.RI.Contracts.Entities
{
    public record CurrentPrice
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("instrumentId")]
        public Guid InstrumentId { get; set; }

        [JsonPropertyName("type")]
        public string Provider { get; set; }

        [JsonPropertyName("last")]
        KeyValuePair<string, OperationDetails> Last { get; set; }

        [JsonPropertyName("ask")]
        KeyValuePair<string, OperationDetails> Ask { get; set; }

        [JsonPropertyName("bid")]
        KeyValuePair<string, OperationDetails> Bid { get; set; }
    }

    public record OperationDetails
    {
        [JsonPropertyName("bid")]
        public DateTime Time { get; set; }

        [JsonPropertyName("price")]
        public Decimal Price { get; set; }

        [JsonPropertyName("volume")]
        public int Volume { get; set; }

        [JsonPropertyName("change")]
        public Decimal Change { get; set; }

        [JsonPropertyName("changePct")]
        public Decimal ChangePct { get; set; }
    }
}
