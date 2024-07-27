using Microsoft.EntityFrameworkCore;

namespace MarketMonitor.RI.Contracts.DbEntities
{
    public record DbAsset
    {
        public Guid Id { get; set; }
        public string? Symbol { get; set; }
        public string? Kind { get; set; }
        public string? Exchange { get; set; }
        public string? Description { get; set; }

        [Precision(18, 2)]
        public decimal TickSize { get; set; }
        public string? Currency { get; set; }
        public string? BaseCurrency { get; set; }
        public IEnumerable<DbMapping> DbMappings { get; set; }

        public List<DbPrice> DbPrices { get; set; }
    }
}
