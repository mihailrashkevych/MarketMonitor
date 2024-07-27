using System.IO;
using System.Text.Json.Serialization;

namespace MarketMonitor.RI.Contracts.Entities
{
    public record WSInMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("instrumentId")]
        public Guid InstrumentId { get; set; }

        [JsonPropertyName("provider")]
        public string Provider { get; set; }
        public string Kind { get; set; }

        [JsonPropertyName("last")]
        public PriceInfo LastInfo { get; set; }

        [JsonPropertyName("ask")]
        public PriceInfo AskInfo { get; set; }

        [JsonPropertyName("bid")]
        public PriceInfo BidInfo { get; set; }
    }

    public class PriceInfo()
    {
        [JsonPropertyName("timestamp")]
        public DateTime Time { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("volume")]
        public int Volume { get; set; }

        [JsonPropertyName("change")]
        public decimal Change { get; set; }

        [JsonPropertyName("changePct")]
        public decimal ChangePercents { get; set; }
    }
}
