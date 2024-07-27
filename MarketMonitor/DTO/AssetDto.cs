namespace MarketMonitor.API
{
    public record AssetDto
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; }
        public string Kind { get; set; }
        public string Exchange { get; set; }
        public string Description { get; set; }
        public decimal TickSize { get; set; }
        public string Currency { get; set; }
        public string? BaseCurrency { get; set; }
        public IEnumerable<MappingDto>? MappingsDto { get; set; }
    }
}
