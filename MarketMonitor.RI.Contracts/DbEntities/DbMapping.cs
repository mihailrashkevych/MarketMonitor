using Microsoft.EntityFrameworkCore;
namespace MarketMonitor.RI.Contracts.DbEntities
{
    public record DbMapping
    {
        public Guid Id { get; set; }
        public string? Symbol { get; set; }
        public int DefaultOrderSize { get; set; }
        public string? ExchangeName { get; set; }
        public string? ProviderName { get; set; }

        public DbAsset DbAsset { get; set; }
    }
}