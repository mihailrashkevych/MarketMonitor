namespace MarketMonitor.API.DTO
{
    public record WSOutMessageDto
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public Guid InstrumentId { get; set; }
        public string Provider { get; set; }
        public bool Subscribe { get; set; }
        public IEnumerable<string> Kinds { get; set; }
    }
}
