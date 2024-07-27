namespace MarketMonitor.Core.Contracts.Models
{
    public record MappingModel
    {
        public Guid Id { get; set; }
        public string? Symbol { get; set; }
        public int DefaultOrderSize { get; set; }
        public string? ExchangeName { get; set; }
        public string? ProviderName { get; set; }
    }
}