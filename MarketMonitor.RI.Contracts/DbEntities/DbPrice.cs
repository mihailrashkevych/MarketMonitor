using Microsoft.EntityFrameworkCore;

namespace MarketMonitor.RI.Contracts.DbEntities
{
    public record DbPrice
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int Volume { get; set; }
        [Precision(18, 2)]
        public decimal Change { get; set; }
        [Precision(18, 2)]
        public decimal ChangePercents { get; set; }

        public string Kind { get; set; }

        public string Type { get; set; }

        public string Provider { get; set; }

        public DbAsset DbAsset { get; set; }
    }
}
