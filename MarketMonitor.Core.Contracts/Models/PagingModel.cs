namespace MarketMonitor.Core.Contracts.Models
{
    public record PagingModel
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int Items { get; set; }
    }
}
