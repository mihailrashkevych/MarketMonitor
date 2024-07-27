namespace MarketMonitor.API
{
    public record MappingDto
    {
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        public int DefaultOrderSize { get; set; }
        public string? ProviderName { get; set; }
    }
}